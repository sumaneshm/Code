import torch
from model import model 

rain_values = ["Yes", "No"]
sprinker_values = ["On", "Off"]
grass_wet_values = ["Wet", "Dry"]

probability = model.probability(
    torch.as_tensor(
        [
            [
                rain_values.index("Yes"),
                sprinker_values.index("Off"),
                grass_wet_values.index("Wet"),
            ]
        ]
    )
)

print(probability)

# Obsidian reference [[Inference in Probablity#ChatGPT example]]
# This code doesn't work fully but it is able to calculate the probability of the grass being wet given the rain and sprinkler states.
# Not fully working because don't understand how to code for the exact question being asked