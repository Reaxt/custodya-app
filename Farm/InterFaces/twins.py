from abc import ABC, abstractmethod

class ITwinSubscriber(ABC):
    """A class to allow classes to subscribe to twin properties.
    """
    @abstractmethod
    def handle_desired(self, data:dict):
        """Handle receiving new desired info

        Args:
            data (dict): the json object as a python dictionary
        """
        pass
    @abstractmethod
    def generate_report(self, data:dict) -> dict:
        """Generate this class's report if possible

        Args:
            data (dict): a dict of possibly other elements. add yours to this.

        Returns:
            dict: the passed object modified to include your additions
        """
        pass