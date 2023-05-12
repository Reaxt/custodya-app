import os
from typing import Union
from dotenv import load_dotenv
from os.path import exists
class Configuration:

    CONN_STR_KEY = "IOTHUB_DEVICE_CONNECTION_STRING"
    SECURITY_KEY = "SECURITY"
    PLANTS_KEY = "PLANTS"
    GEOLOCATION_KEY = "GEOLOCATION"
    def __init__(self) -> None:
        """Initialise a configuration object, loaded from the .env OR environment variables

        Args:
            iot_connection_string (str): The IOT Hub connection string
            security (bool): If we are using a security subsystem
            plants (bool): If we are using a plants subsystem
            geolocation (bool): If we are using a geolocation subsystem
        """
        if(exists(".env")):
            load_dotenv()
        else:
            print("Warning: No .env file found. Make sure you have the appropriate environment variables set another way!")
        self.iot_connection_string:Union[str,None] = os.environ.get(Configuration.CONN_STR_KEY)
        if self.iot_connection_string is None:
            raise EnvironmentError("Missing connection environment variable!")
        self.geolocation_enabled = GetBoolEnvFlag(Configuration.GEOLOCATION_KEY)
        self.security_enabled = GetBoolEnvFlag(Configuration.SECURITY_KEY)
        self.plants_enabled = GetBoolEnvFlag(Configuration.PLANTS_KEY)


def GetBoolEnvFlag(variable:str) -> bool:
    if(os.environ.get(variable) is None):
        print(f"WARNING! Could not find environment variable {variable}, defaulting to false")
        return False
    else:
        return True if os.environ.get(variable) == "true" else False