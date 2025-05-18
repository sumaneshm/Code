from pomegranate.distributions import *
from pomegranate.bayesian_network import BayesianNetwork

rain = Categorical( 
    [
        [ 0.7, 0.2, 0.1 ]
    ] 
)

maintenance = ConditionalCategorical( 
    [
        [
            [ 0.4, 0.6 ], 
            [ 0.2, 0.8 ], 
            [ 0.1, 0.9 ]
        ]
    ]
)

train = ConditionalCategorical(
    [
        [
            [
                [0.8, 0.2],
                [0.9, 0.1],
            ],
            [
                [0.6, 0.4],
                [0.7, 0.3],
            ],
            [
                [0.4, 0.6],
                [0.5, 0.5],
            ],
        ]
    ]
)


appointment = ConditionalCategorical(
    [
        [
            [0.9, 0.1],
            [0.6, 0.4],
        ],
    ]
)


# Create a Bayesian Network and add states
model = BayesianNetwork()
model.add_distributions([rain, maintenance, train, appointment])

# Add edges connecting nodes
model.add_edge(rain, maintenance)
model.add_edge(rain, train)
model.add_edge(maintenance, train)
model.add_edge(train, appointment)