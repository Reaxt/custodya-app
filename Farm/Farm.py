from security.SecuritySubSystem import Security
from plants.plantsSubSystem import Plants
from GeoLocation.GeoLocationSubSystem import GeoLocation 

class Farm:
    def __init__(self) -> None:
        securitySubSystem = Security()
        plantsSubSystem = Plants()
        geolocation = GeoLocation()
        self.subsystems = list()
        self.subsystems.append(securitySubSystem,plantsSubSystem,geolocation)

    def read_sensors(self):
        for subsystem in self.subsystems:
            print(subsystem.read_sensors())