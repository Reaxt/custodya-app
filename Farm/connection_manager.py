import asyncio
import json
from configuration_manager import Configuration
from datetime import datetime
from azure.iot.device.aio import IoTHubDeviceClient
from azure.iot.device import MethodRequest, MethodResponse, Message
from typing import Any, Callable, Dict
from InterFaces.sensors import AReading

class ConnectionManager:
    """ Responsible for talking with the IOT hub.
    """
    CONN_STR_KEY = "IOTHUB_DEVICE_CONNECTION_STRING"

    def __init__(self, configuration:Configuration):
        conn_string = configuration.iot_connection_string
        self.client: IoTHubDeviceClient = IoTHubDeviceClient.create_from_connection_string(conn_string)
        self.connected: bool = False
        self._commands: Dict[str, Callable[[MethodRequest], MethodResponse]] = dict()
        
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

    def set_twin_handler(self, handler: Callable[[dict],None]):
        """Set the twin handler method"""
        self.client.on_twin_desired_properties_patch_received = handler
    def subscribe_method_request(self, method:str, handler:Callable[[MethodRequest], MethodResponse]):
        """Set a handler method for a specific direct method"""
        self._commands[method] = handler
    async def connect(self):
        """Connect to the IOT hub"""
        """Connected to the IoT hub"""
        await self.client.connect()
        self.connected = True
    async def send_telemetry(self, telemetry:dict[str, Any]):
        jsonstr = json.dumps(telemetry)
        print(jsonstr)
        msg = Message(jsonstr)
        await self.client.send_message(msg)