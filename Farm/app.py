import asyncio
import json
import argparse
from configuration_manager import Configuration
from connection_manager import ConnectionManager
from Farm import Farm
from azure.iot.device import MethodResponse, MethodRequest

from twinsubscribers.control_manager import ControlManager
from twinsubscribers.telemetry_interval import TelemtryIntervalSubscriber

parser = argparse.ArgumentParser()
parser.add_argument("--logtelemetry", type=bool)
async def main():
    args = parser.parse_args()
    TELEMETRY_TIME = 5
    configuration: Configuration = Configuration()
    connection_manager: ConnectionManager = ConnectionManager(configuration)

    connection_manager.subscribe_method_request("is_online", is_online_handler)

    #connection_manager.add_twin_handler(telemetry_patch_handler)

    farm = Farm(configuration)
    control_manager = ControlManager(farm)
    telemetry_time = TelemtryIntervalSubscriber(TELEMETRY_TIME)
    
    connection_manager.add_twin_handler(control_manager)
    connection_manager.add_twin_handler(telemetry_time)
    
    await connection_manager.connect()

    
    async def telemetryloop():
        while True:
            state = farm.serialize_state()
            if args.logtelemetry:
                print(json.dumps(state))
            await connection_manager.send_telemetry(state)
            await asyncio.sleep(telemetry_time.get_interval())
    await telemetryloop()

def is_online_handler(method_request: MethodRequest) -> MethodResponse:
    payload = {"result":True}
    status = 200
    return MethodResponse.create_from_method_request(method_request, status, payload)


if __name__ == "__main__":
    
    asyncio.run(main())