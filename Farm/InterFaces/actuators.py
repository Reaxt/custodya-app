from abc import ABC, abstractmethod
from enum import Enum
import json
from typing import Any, Union

class ACommand:
    """Class for actuator commands. The type of the command has to be handled by the actuator itself."""
    def __init__(self, target: str, body: Union[str, dict]) -> None:
        """Initialises an actuator command
        
        :param str target: The type of actuator this will target.
        :param str raw_message_body: Body of the message as a json string OR parsed dict """
        self.target_type = target
        parsed_data:dict
        if isinstance(body, str):
            parsed_data = json.loads(body) # type: ignore
        else:
            parsed_data = body # type: ignore
        self.data:dict = parsed_data


    def __repr__(self) -> str:
        return f'Command for {self.target_type} as {self.data}'
class IActuator(ABC):

    # Properties to be initialized in constructor of implementation classes
    _current_state: dict

    @abstractmethod
    def __init__(self, gpio: int, initial_state: dict) -> None:
        """Constructor for Actuator class. Must define interface's class properties

        :param dict initial_state: initializes 'current_state' property of a new actuator.
        Should minimally include the key "value" with corresponding value of initial state
        """
        pass

    @abstractmethod
    def validate_command(self, command: ACommand) -> bool:
        """Validates that a command can be used with the specific actuator.

        :param ACommand command: the command to be validated.
        :return bool: True if command can be consumed by the actuator.
        """
        pass

    @abstractmethod
    def control_actuator(self, data: dict) -> bool:
        """Sets the actuator to the value passed as argument.

        :param dict value: dictionary containing keys and values with command information.
        :return bool: True if the state of the actuator changed, false otherwise.
        """
        pass
    def try_command(self, command:ACommand) -> bool:
        """Validates if a command works here, and runs it if it can.
        :param ACommand the command to run
        :return bool: True if the command succeeded, false if the command was not ran for any reason"""
        if self.validate_command(command):
            return self.control_actuator(command.data)
        else:
            return False
    @abstractmethod
    def get_current_state(self) -> Any:
        """Get the current state of the actuator in a way that works for serialization

        Returns:
            Any: The actuators current state.
        """
        pass
    @abstractmethod
    def get_actuator_name(self) -> str:
        """Get the name of this actuator for serialization

        Returns:
            str: the name of this actuator.
        """
        pass