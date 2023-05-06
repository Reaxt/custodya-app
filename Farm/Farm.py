from subsystems.security.SecuritySubSystem import SecuritySubSystem
from subsystems.plants.PlantsSubSystem import PlantsSubSystem
from subsystems.GeoLocation.GeoLocationSubSystem import GeoLocation 

class Farm:
    def __init__(self) -> None:
        geolocation = GeoLocation()

        securitySubSystem = SecuritySubSystem()
        plantsSubSystem = PlantsSubSystem()

        self.subsystems = list()
        #self.subsystems.append(securitySubSystem)
        #self.subsystems.append(plantsSubSystem)
        self.subsystems.append(geolocation)
    def read_sensors(self):
        for subsystem in self.subsystems:
            print(subsystem.read_sensors())


def main():
   farm = Farm()
   farm.read_sensors()

if __name__ == "__main__":
    main()