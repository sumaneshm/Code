from pomegranate.distributions import *
from pomegranate.bayesian_network import BayesianNetwork

rain = Categorical( 
    [
        [ 0.2, 0.8 ]
    ] 
)

sprinker = Categorical (
    [ 
        [0.1, 0.9]
    ]
)

grass_wet = ConditionalCategorical( 
    [
        [
            [
                [0.99, 0.01],
                [0.9, 0.1],
            ],
            [
                [0.8, 0.2],
                [0.0, 1.0],
            ],
        ]
    ]
)

model = BayesianNetwork()

model.add_distributions([rain, sprinker, grass_wet])
model.add_edge(rain, grass_wet)
model.add_edge(sprinker, grass_wet)

# Obsidian reference [[Inference in Probablity#ChatGPT example]]