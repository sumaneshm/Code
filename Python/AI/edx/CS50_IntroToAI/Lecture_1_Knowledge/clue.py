"""
Clue game logic implementation
"""

import termcolor
from logic import Symbol, And, Not, Or, ModelChecker

# Define suspects
sumanesh = Symbol("Sumanesh")
arun = Symbol("Arun")
kumar = Symbol("Kumar")

# Define locations
kitchen = Symbol("Kitchen")
bedroom = Symbol("Bedroom")
study = Symbol("Study")

# Define weapons
knife = Symbol("Knife")
revolver = Symbol("Revolver")
wrench = Symbol("Wrench")

# Create lists of symbols
people = [sumanesh, arun, kumar]
rooms = [kitchen, bedroom, study]
weapons = [knife, revolver, wrench]
symbols = people + rooms + weapons

# Create initial knowledge
knowledge = And(
    # Exactly one suspect
    Or(sumanesh, arun, kumar),
    Not(And(sumanesh, arun)),
    Not(And(sumanesh, kumar)),
    Not(And(arun, kumar)),
    
    # Exactly one location
    Or(kitchen, bedroom, study),
    Not(And(kitchen, bedroom)),
    Not(And(kitchen, study)),
    Not(And(bedroom, study)),
    
    # Exactly one weapon
    Or(knife, revolver, wrench),
    Not(And(knife, revolver)),
    Not(And(knife, wrench)),
    Not(And(revolver, wrench))
)

# Add known facts
knowledge.add(And(
    Not(sumanesh),  # Sumanesh is not the suspect
    Not(kitchen),  # Not in kitchen
    Not(knife)    # Not using knife
))

def check(knowledge):
    """
    Check knowledge base and print results for each symbol
    """
    checker = ModelChecker([knowledge])
    for symbol in symbols:
        result = checker.evaluate_models(symbol)
        if result is True:
            termcolor.cprint(f"{symbol}: YES", "green")
        elif result is False:
            termcolor.cprint(f"{symbol}: NO", "red")
        else:  # result is None (MAYBE)
            termcolor.cprint(f"{symbol}: MAYBE", "yellow")

if __name__ == "__main__":
    check(knowledge)
