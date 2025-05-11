import itertools
class Sentence:
    def evaluate(self):
        raise NotImplementedError("Subclasses should implement this method.")
    def __str__(self):
        raise NotImplementedError("Subclasses should implement this method.")
    
    def __hash__(self):
        return hash(self.__class__.__name__) + hash(tuple(self.__dict__.items()))
        
    def __eq__(self, value):    
        return isinstance(value, self.__class__) and self.__hash__() == value.__hash__()
    
    def __ne__(self, value):
        return not self.__eq__(value)   

    def all_symbols(self):
        return set()

class Symbol(Sentence):
    def __init__(self, name):
        self.name = name

    def __eq__(self, value):
        return self.name == value.name and super().__eq__(value)
    
    def evaluate(self, model):
        return self.name in model and model[self.name]

    def __str__(self):
        return self.name
    
    def __hash__(self):
        return hash(self.name)
    
    def all_symbols(self):
        return {self.name}

class And(Sentence):
    def __init__(self, *args):
        self.args = args

    def __eq__(self, value):
        return self.args == value.args and super().__eq__(value)
    
    def evaluate(self, model):
        return all(arg.evaluate(model) for arg in self.args)

    def __str__(self):
        return "(" +  " AND ".join(str(arg) for arg in self.args) 
    
    def __hash__(self):
        return hash(tuple(self.args))
    
    def all_symbols(self):
        return set().union(*(arg.all_symbols() for arg in self.args))   
   
class Or(Sentence):
    def __init__(self, *args):
        self.args = args

    def __eq__(self, value):
        return self.args == value.args and super().__eq__(value)
    
    def evaluate(self, model):
        return any(arg.evaluate(model) for arg in self.args)

    def __str__(self):
        return "(" + " OR ".join(str(arg) for arg in self.args) + ")"
    
    def __hash__(self):
        return hash(tuple(self.args))
    
    def all_symbols(self):
        return set().union(*(arg.all_symbols() for arg in self.args))

class Not(Sentence):    
    def __init__(self, arg):
        self.arg = arg

    def __eq__(self, value):
        return value.arg == self.arg and  super().__eq__(value) 
    
    def evaluate(self, model):
        return not self.arg.evaluate(model)

    def __str__(self):
        return f"( NOT {self.arg} )"
    
    def __hash__(self):
        return hash(self.arg)   
    
    def all_symbols(self):  
        return self.arg.all_symbols()
    

class Implication(Sentence):
    def __init__(self, antecedent, consequent):
        self.antecedent = antecedent
        self.consequent = consequent

    def __eq__(self, value):
        return value.antecedent == self.antecedent and value.consequent == self.consequent and super().__eq__(value)
    
    def evaluate(self, model):
        return not self.antecedent.evaluate(model) or self.consequent.evaluate(model)

    def __str__(self):
        return f"( {self.antecedent} => {self.consequent} )"
    
    def __hash__(self):
        return hash((self.antecedent, self.consequent))
    
    def all_symbols(self):
        return self.antecedent.all_symbols().union(self.consequent.all_symbols())
    
class Biconditional(Sentence):  
    def __init__(self, left, right):
        self.left = left
        self.right = right

    def __eq__(self, value):
        return value.left == self.left and value.right == self.right and super().__eq__(value)
    
    def evaluate(self, model):
        return self.left.evaluate(model) == self.right.evaluate(model)

    def __str__(self):
        return f"( {self.left} <=> {self.right} ) "
    
    def __hash__(self):
        return hash((self.left, self.right))
    
    def all_symbols(self):  
        return self.left.all_symbols().union(self.right.all_symbols())
    
class ModelChecker: 
    def __init__(self, sentences):
        self.sentences = sentences
        self.model = {}

    def assign(self, symbol, value):
        self.model[symbol] = value

    def evaluate(self, model):
        print(f"Evaluating model: {model}")
        for sentence in self.sentences:
            if not sentence.evaluate(model):
                print(f"\tDoes not satisfy sentence: {sentence}")
                return False
            else:
                print(f"\tSatisfies sentence: {sentence}")
        return True

    def __str__(self):
        return str(self.model)

    def all_symbols(self):
        symbols = set()
        for sentence in self.sentences:
            symbols.update(sentence.all_symbols())
        return symbols

    def generate_models(self):
        # Get the number of sentences
        symbols = self.all_symbols()

        # Generate all possible combinations of True and False values
        truth_values = list(itertools.product([True, False], repeat=len(symbols)))

        # Print the models
        models = []

        for i, truth_value in enumerate(truth_values):
            model = dict(zip(symbols, truth_value))
            models.append(model)

        return models 

    def evaluate_models(self):
        for model in self.generate_models():
            if self.evaluate(model):
                print(f"Model {model} satisfies all sentences.")
            else:
                print(f"Model {model} does not satisfy all sentences.")
    
# sentences = [
#     Symbol("A"),
#     Symbol("B"),
#     And(Symbol("A"), Symbol("B")),
#     # Or(Symbol("A"), Not(Symbol("B"))),
#     # Implication(Symbol("A"), Symbol("B")),
#     # Biconditional(Symbol("A"), Symbol("B")), 
#     # And(Symbol("A"), Or(Symbol("B"), Not(Symbol("C")))),
# ]

# # Example usage
# model_checker = ModelChecker(sentences)
# # model_checker.assign("A", True) 
# # model_checker.assign("B", False)
# # model_checker.assign("C", True) 

# # model_checker.evaluate()
# # print("Model:", model_checker)

# # for sentence in sentences:
# #     print(f"Sentence: {sentence}, Evaluation: {sentence.evaluate()}")


# # for symbol in model_checker.all_symbols():
# #     print(f"Symbol: {symbol}")

# # model_checker.generate_models()

# model_checker.evaluate_models()

rain = Symbol("Rain")
salem= Symbol("Salem")
visit_salem = Implication(rain, salem)
not_visit_salem = Implication(Not(rain), Not(salem))
biconditional = Biconditional(visit_salem, not_visit_salem)    
visit_salem_not_rain = Implication(salem, Not(rain))

# single = And ( visit_salem, not_visit_salem, biconditional, visit_salem_not_rain)

ModelChecker([visit_salem, not_visit_salem, biconditional, visit_salem_not_rain]).evaluate_models()

# ModelChecker([single]).evaluate_models()

# ModelChecker([visit_salem, not_visit_salem, biconditional, visit_salem_not_rain]).evaluate_models()