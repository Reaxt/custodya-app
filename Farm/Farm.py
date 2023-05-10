import asyncio
import os
from dotenv import load_dotenv
from configuration_manager import Configuration
from subsystems.security.SecuritySubSystem import SecuritySubSystem
from subsystems.plants.plantsSubSystem import PlantsSubSystem
from subsystems.GeoLocation.GeoLocationSubSystem import GeoLocation 
from azure.iot.device.aio import IoTHubDeviceClient
from azure.iot.device import MethodResponse, MethodRequest
from azure.iot.device import Message
from InterFaces.sensors import AReading
from connection_manager import ConnectionManager
class Farm:
    def __init__(self, configuration:Configuration) -> None:
        
        self.subsystems = dict()
        if(configuration.geolocation_enabled):
            self.subsystems["geolocation"] = GeoLocation()
        else:
            self.subsystems["geolocation"] = None
        if(configuration.security_enabled):
            self.subsystems["security"] = SecuritySubSystem()
        else:
            self.subsystems["security"] = None
        
        if(configuration.plants_enabled):
            self.subsystems["plants"] = PlantsSubSystem()
        else:
            self.subsystems["plants"] = None
    def read_sensors(self) -> dict[str, list[AReading]]:
        readings = dict()
        for key in self.subsystems:
            if self.subsystems[key] != None:
                readings[key]=(self.subsystems[key].read_sensors())
        return readings

async def main():
    TELEMETRY_TIME = 5
    configuration: Configuration = Configuration()
    connection_manager: ConnectionManager = ConnectionManager(configuration)

    connection_manager.subscribe_method_request("is_online", is_online_handler)
    def twin_patch_handler(patch: dict):
        print(f"Patch received: {patch}")        
        try:
            telemetryInterval = patch["telemetryInterval"]
            if telemetryInterval.isnumeric():
                TELEMETRY_TIME = int(telemetryInterval)
        except:
            TELEMETRY_TIME = 5
    connection_manager.set_twin_handler(twin_patch_handler)

    await connection_manager.connect()

    farm = Farm(configuration)
    
    async def telemetryloop():
        while True:
            await connection_manager.send_telemetry(farm.read_sensors())
            await asyncio.sleep(TELEMETRY_TIME)
    await telemetryloop()

def is_online_handler(method_request: MethodRequest) -> MethodResponse:
    payload = {"result":True}
    status = 200
    return MethodResponse.create_from_method_request(method_request, status, payload)


if __name__ == "__main__":
    asyncio.run(main())