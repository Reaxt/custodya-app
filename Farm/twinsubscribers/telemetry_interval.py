from InterFaces.twins import ITwinSubscriber

class TelemtryIntervalSubscriber(ITwinSubscriber):
    def __init__(self, default:int = 5) -> None:
        super().__init__()
        self._telemetry = default;
    def handle_desired(self, data: dict) -> None:
        self._telemetry = data["telemetryInterval"]
    def generate_report(self, data: dict) -> dict:
        data["telemetryInterval"] = self._telemetry
        return data
    def get_interval(self) -> int:
        return self._telemetry;