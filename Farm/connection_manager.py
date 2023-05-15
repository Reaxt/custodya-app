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
    CONN_STR_KEY = "IOTHUB_DEVICE_CONNECTION_STRING"

    def __init__(self, configuration:Configuration):
        conn_string = configuration.iot_connection_string
        self.client: IoTHubDeviceClient = IoTHubDeviceClient.create_from_connection_string(conn_string)
        self.connected: bool = False
        self._commands: Dict[str, Callable[[MethodRequest], MethodResponse]] = dict()
        self._twin_handlers: list[ITwinSubscriber] = list()

        async def method_request_handler(method_request:MethodRequest):
            method_response:MethodResponse
            if any (method_request.name in key for key in self._commands):
                method_response = self._commands[method_request.name](method_request)
            else:
                payload = {"details":"method name unknown"}
                status = 400
                method_response = MethodResponse.create_from_method_request(method_request, status, payload)
            await self.client.send_method_response(method_response)
        def twin_property_handler(data:dict):
            print(f"received {data}")
            print(f"providing {data}")
            for handler in self._twin_handlers:
                print(f"serving {handler}")
                handler.handle_desired(data)
        self.client.on_twin_desired_properties_patch_received = twin_property_handler

        self.client.on_method_request_received = method_request_handler

    def add_twin_handler(self, handler: ITwinSubscriber):
        """Adds a callable to be ran when we receive twin updates.

        Args:
            handler (ITwinSubscriber): Twin handler.
        """
        self._twin_handlers.append(handler)

    def subscribe_method_request(self, method:str, handler:Callable[[MethodRequest], MethodResponse]):
        """Set a handler method for a specific direct method"""
        self._commands[method] = handler

    async def connect(self):
        """Connect to the IOT hub"""
        """Connected to the IoT hub"""
        await self.client.connect()
        self.connected = True
        await self.client.get_twin()

    async def send_telemetry(self, telemetry:dict[str, Any]):
        jsonstr = json.dumps(telemetry)
        print(jsonstr)
        msg = Message(jsonstr)
        await self.client.send_message(msg)