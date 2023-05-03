import security 
import plants
import Geolocation 

class Farm:
    def __init__(self) -> None:
        securitySubSystem = security()
        plantsSubSystem = plants()
        geolocation = Geolocation()
        self.subsystems = list()
        self.subsystems.append(securitySubSystem,plantsSubSystem,geolocation)

    def read_sensors(self):
        for subsystem in self.subsystems:
            print(subsystem.read_sensors())