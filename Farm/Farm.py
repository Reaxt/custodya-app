from subsystems.security import SecuritySubSystem
from subsystems.plants import PlantsSubSystem
from subsystems.GeoLocation.GeoLocationSubSystem import GeoLocation 

class Farm:
    def __init__(self) -> None:
        plantsSubSystem = PlantsSubSystem()
        securitySubSystem = SecuritySubSystem()
        geolocation = GeoLocation()

        self.subsystems = list()
        self.subsystems.append(securitySubSystem,plantsSubSystem,geolocation)

    def read_sensors(self):
        for subsystem in self.subsystems:
            print(subsystem.read_sensors())