import asyncio
import json
from InterFaces.twins import ITwinSubscriber
from configuration_manager import Configuration
from datetime import datetime
from azure.iot.device.aio import IoTHubDeviceClient
from azure.iot.device import MethodRequest, MethodResponse, Message
from typing import Any, Callable, Dict
from InterFaces.sensors import AReading
from InterFaces.twins import ITwinSubscriber

class ConnectionManager:
    """ Responsible for talking with the IOT hub.
    """
    REPORT_RATE_KEY = "reportUpdateRate"
    DEFAULT_REPORT_RATE = 10
    def __init__(self, configuration:Configuration):
        conn_string = configuration.iot_connection_string
        self.client: IoTHubDeviceClient = IoTHubDeviceClient.create_from_connection_string(conn_string)
        self.connected: bool = False
        self._commands: Dict[str, Callable[[MethodRequest], MethodResponse]] = dict()
        self._twin_handlers: list[ITwinSubscriber] = list()
        self._report_sleep = ConnectionManager.DEFAULT_REPORT_RATE
        
        async def method_request_handler(method_request:MethodRequest):
            method_response:MethodResponse
            if any (method_request.name in key for key in self._commands):
                method_response = self._commands[method_request.name](method_request)
            else:
                payload = {"details":"method name unknown"}
                status = 400
                method_response = MethodResponse.create_from_method_request(method_request, status, payload)
            await self.client.send_method_response(method_response)



        self.client.on_method_request_received = method_request_handler

    async def twin_property_handler(self, data:dict):
        print(f"received twin patch")
        #it would be nice to have this handle partial updates, but for now we will just refetch our desired properties each time.
        data = (await self.client.get_twin())["desired"]
        if ConnectionManager.REPORT_RATE_KEY in data:
            self._report_sleep = data[ConnectionManager.REPORT_RATE_KEY]
        for handler in self._twin_handlers:
            print(f"serving {handler}")
            handler.handle_desired(data)
        await self.report_twin() #if we JUST changed something, we should update our report.
    async def report_loop(self):
        #does this need a cancellation token?
        while True:
            await asyncio.sleep(self._report_sleep)
            report = {
                ConnectionManager.REPORT_RATE_KEY: self._report_sleep
            }
            for subscriber in self._twin_handlers:
                report = subscriber.generate_report(report)
            await self.client.patch_twin_reported_properties(report)
    async def report_twin(self):
        report = {
            ConnectionManager.REPORT_RATE_KEY: self._report_sleep
        }
        for subscriber in self._twin_handlers:
            report = subscriber.generate_report(report)
        await self.client.patch_twin_reported_properties(report)
    def add_twin_handler(self, handler: ITwinSubscriber):
        """Adds a callable to be ran when we receive twin updates.

        Args:
            handler (ITwinSubscriber): Twin handler.
        """
        print(f"adding twin handler {handler}")
        self._twin_handlers.append(handler)

    def subscribe_method_request(self, method:str, handler:Callable[[MethodRequest], MethodResponse]):
        """Set a handler method for a specific direct method"""
        self._commands[method] = handler

    async def connect(self):
        """Connect to the IOT hub"""
        self.client.on_twin_desired_properties_patch_received = self.twin_property_handler
        print("connecting..")

        await self.client.connect()
        print("connected")
        self.connected = True
        twin = await self.client.get_twin()
        #init
        await self.twin_property_handler(twin["desired"])
        self._report_task = asyncio.create_task(self.report_loop())

    async def send_telemetry(self, telemetry:dict[str, Any]):
        jsonstr = json.dumps(telemetry)
        msg = Message(jsonstr)
        await self.client.send_message(msg)