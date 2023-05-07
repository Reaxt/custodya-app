import asyncio
import json
import os
from dotenv import load_dotenv
from subsystems.security.SecuritySubSystem import SecuritySubSystem
from subsystems.plants.plantsSubSystem import PlantsSubSystem
from subsystems.GeoLocation.GeoLocationSubSystem import GeoLocation 
from azure.iot.device.aio import IoTHubDeviceClient
from azure.iot.device import MethodResponse
from azure.iot.device import Message
from InterFaces.sensors import AReading
class Farm:
    def __init__(self) -> None:
        geolocation = GeoLocation()

        securitySubSystem = SecuritySubSystem()
        plantsSubSystem = PlantsSubSystem()
        
        self.subsystems = list()
        self.subsystems.append(securitySubSystem)
        #self.subsystems.append(plantsSubSystem)
        #self.subsystems.append(geolocation)
    def read_sensors(self) -> list[AReading]:
        readings = list()
        for subsystem in self.subsystems:
            readings.extend(subsystem.read_sensors())
        return readings

async def main():
    TELEMETRY_TIME = 5
    load_dotenv()
    loop = asyncio.get_event_loop()
    conn_str = os.environ["IOTHUB_DEVICE_CONNECTION_STRING"]
    device_client = IoTHubDeviceClient.create_from_connection_string(conn_str)   
    await device_client.connect()
    farm = Farm()
    
    async def telemetryloop():
        while True:
            data = readings_to_json(farm.read_sensors())
            msg = Message(data)
            await device_client.send_message(msg)
            await asyncio.sleep(TELEMETRY_TIME)
    
    #loop.create_task(telemetryloop())
    async def method_request_handler(method_request):
        payload = None
        if method_request.name == "is_online":
            payload = {"result":True}
            status = 200
        else:
            payload = {"details":"method name unknown"}
            status = 400
        method_response = MethodResponse.create_from_method_request(method_request, status, payload)
        await device_client.send_method_response(method_response)

    def twin_patch_handler(patch):
        print(f"Patch received: {patch}")        
        try:
            telemetryInterval = patch["telemetryInterval"]
            if telemetryInterval.isnumeric():
                TELEMETRY_TIME = int(telemetryInterval)
        except:
            TELEMETRY_TIME = 5        
    
    device_client.on_twin_desired_properties_patch_received = twin_patch_handler
    device_client.on_method_request_received = method_request_handler
    await telemetryloop()

def readings_to_json(readings:list[AReading]) -> str:
    values = []
    for reading in readings:
        obj = {"type":reading.reading_type, "unit":reading.reading_unit, "value":reading.value}
        values.append(obj)
    print(values)
    return json.dumps(values)

if __name__ == "__main__":
    asyncio.run(main())