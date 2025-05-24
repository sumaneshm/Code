import numpy
import torch
from .model import model 


def execute_code():
    rain_values = ["none", "light", "heavy"]
    maintenance_values = ["yes", "no"]
    train_values = ["on time", "delayed"]
    appoinment_values = ["attend", "miss"]


    probability = model.probability(
        torch.as_tensor(
            [
                [
                    rain_values.index("none"),
                    maintenance_values.index("no"),
                    train_values.index("on time"),
                    appoinment_values.index("miss"),
                ]
            ]
        )
    )

    print(probability)

    # Obsidian reference [[Inference in Probablity#How to infer from Bayesian Network diagram]]