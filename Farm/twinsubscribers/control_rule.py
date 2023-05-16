
from enum import Enum
from typing import Any, Union
from InterFaces.actuators import ACommand

from InterFaces.sensors import AReading


class Comparisons(str, Enum):
    """Enum defining the comparison methods available"""
    EQUAL = "=="
    GREATER = ">"
    LESS = "<"
class RuleKeys(str, Enum):
    """An enum to hold all the key types in a rules object."""
    READING_KEY = "targetReadingType"
    VALUE_KEY = "targetValue"
    COMPARISON_KEY = "comparisonType",
    RESULT_KEY = "valueOnRule"

class ControlRule():
    REQUIRED_KEYS = [
        RuleKeys.READING_KEY,
        RuleKeys.VALUE_KEY,
        RuleKeys.COMPARISON_KEY,
        RuleKeys.RESULT_KEY ]
    def __init__(self, ruleBody: dict, target_actuator: str) -> None:
        self._target_actuator = target_actuator
        #region validation
        for k in ControlRule.REQUIRED_KEYS:
            if ruleBody[k] == None:
                raise KeyError(f"Rule missing key {k}")
        if ruleBody[RuleKeys.COMPARISON_KEY] not in list(Comparisons):
            raise ValueError(f"Comparison key \"{ruleBody[RuleKeys.COMPARISON_KEY]}\" is invalid!")
        if ruleBody[RuleKeys.READING_KEY] not in list(AReading.ReadingType):
            raise ValueError(f"Reading type \"{ruleBody[RuleKeys.READING_KEY]}\" is invalid!")
        #endregion
        self._comparison: Comparisons = Comparisons(ruleBody[RuleKeys.COMPARISON_KEY])
        self._reading_type: AReading.ReadingType = AReading.ReadingType(ruleBody[RuleKeys.READING_KEY])
        self._test_value: Any = ruleBody[RuleKeys.VALUE_KEY]
        self._result_value: Any = ruleBody[RuleKeys.RESULT_KEY]
    def generate_command(self, readings:list[AReading]) -> Union[ACommand, None]:
        """Return an ACommand or None depending on the result of this rule.

        Args:
            readings (list[AReading]): All current readings of the farm.

        Returns:
            Union[ACommand, None]: ACommand if the check resulted in True, None otherwise.
        """
        reading: Union[None, AReading] = None
        for read in readings:
            if read.reading_type == self._reading_type:
                reading = read
        if reading == None:
            print(f"Could not enforce rule: reading {self._reading_type} not found!")
            return None

        comparisonRes:bool = False 
        if self._comparison == Comparisons.EQUAL:
            comparisonRes = self._test_value == reading.value
        elif self._comparison == Comparisons.GREATER:
            comparisonRes = reading.value > self._test_value
        elif self._comparison == Comparisons.LESS:
            comparisonRes = reading.value < self._test_value
        if comparisonRes == False:
            return None
        return ACommand(self._target_actuator, {"value": self._result_value})
    def serialize(self) -> dict:
        """Serialize this rules data into a dictionary.

        Returns:
            dict: The data this rule follows
        """
        return {
            RuleKeys.READING_KEY: self._reading_type,
            RuleKeys.VALUE_KEY: self._test_value,
            RuleKeys.COMPARISON_KEY: self._comparison,
            RuleKeys.RESULT_KEY: self._result_value
        }