import json
from dataclasses import dataclass, field
from typing import Tuple, Dict

@dataclass
class Produce:
    name: str
    ideal_temp_range: Tuple[float, float]  # (min_temp, max_temp)
    max_health: int
    freshness_decay_per_second: float  # Rate at which health decays outside temp
    textures: Dict[str, str]  # e.g. {"fresh": "apple_fresh.png", "rotten": "apple_rotten.png"}
    current_health: float = field(init=False)
    
    def __post_init__(self):
        self.current_health = self.max_health

    def update_health(self, current_temp: float, delta_time: float):
        """
        Update the health of the produce based on current temperature and elapsed time.

        :param current_temp: The temperature of the grid square the produce is in.
        :param delta_time: Time passed (in seconds) since the last update.
        """
        min_temp, max_temp = self.ideal_temp_range
        if current_temp < min_temp or current_temp > max_temp:
            self.current_health -= self.freshness_decay_per_second * delta_time
            self.current_health = max(0, self.current_health)

    def get_current_texture(self) -> str:
        """
        Return the appropriate texture based on current health.
        """
        health_percent = self.current_health / self.max_health
        if health_percent > 0.5:
            return self.textures["fresh"]
        else:
            return self.textures["rotten"]

    def is_spoiled(self) -> bool:
        """
        Check if the produce is considered spoiled.
        """
        return self.current_health <= 0

    def reset_freshness(self):
        """
        Reset the produce's freshness to its maximum value (used in cleaning stations).
        """
        self.current_health = self.max_health
