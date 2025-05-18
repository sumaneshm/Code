"""
Logic module for symbolic logic operations
"""

import itertools


class Sentence:
    """
    Base class for all logical sentences
    """
    def evaluate(self):
        """Evaluate the sentence"""
        raise NotImplementedError("Subclasses should implement this method.")
    
    def __str__(self):
        """Return string representation of the sentence"""
        raise NotImplementedError("Subclasses should implement this method.")
    
    def __hash__(self):
        """Return hash value for the sentence"""
        return hash(self.__class__.__name__) + hash(tuple(self.__dict__.items()))
    
    def __eq__(self, value):    
        """Check if two sentences are equal"""
        return isinstance(value, self.__class__) and self.__hash__() == value.__hash__()
    
    def __ne__(self, value):
        """Check if two sentences are not equal"""
        return not self.__eq__(value)

    def all_symbols(self):
        """Return set of all symbols in the sentence"""
        return set()

class Symbol(Sentence):
    """
    A basic symbol representing a proposition
    """
    def __init__(self, name):
        """Initialize symbol with name"""
        self.name = name

    def __eq__(self, value):
        """Check if symbols are equal"""
        return self.name == value.name and super().__eq__(value)
    
    def evaluate(self, model):
        """Evaluate symbol in given model"""
        return self.name in model and model[self.name]

    def __str__(self):
        """Return string representation of symbol"""
        return self.name
    
    def __hash__(self):
        """Return hash value for symbol"""
        return hash(self.name)
    
    def all_symbols(self):
        """Return set containing this symbol"""
        return {self.name}

class And(Sentence):
    """
    Logical AND operation
    """
    def __init__(self, *args):
        """Initialize AND with multiple arguments"""
        self.args = list(args)

    def __eq__(self, value):
        """Check if AND sentences are equal"""
        return self.args == value.args and super().__eq__(value)
    
    def evaluate(self, model):
        """Evaluate AND in given model"""
        return all(arg.evaluate(model) for arg in self.args)

    def __str__(self):
        """Return string representation of AND"""
        return "(" + " AND ".join(str(arg) for arg in self.args) + ")"

    def add(self, arg):
        """Add new argument to AND"""
        self.args.append(arg)
    
    def __hash__(self):
        """Return hash value for AND"""
        return hash(tuple(self.args))

    def all_symbols(self):
        """Return set of all symbols in AND"""
        return set().union(*(arg.all_symbols() for arg in self.args))


class Not(Sentence):
    """
    Logical NOT operation
    """

    def __init__(self, arg):
        """Initialize NOT with argument"""
        self.arg = arg

    def __eq__(self, value):
        """Check if NOT sentences are equal"""
        return value.arg == self.arg and super().__eq__(value)

    def evaluate(self, model):
        """Evaluate NOT in given model"""
        return not self.arg.evaluate(model)

    def __str__(self):
        """Return string representation of NOT"""
        return f"( NOT {self.arg} )"

    def __hash__(self):
        """Return hash value for NOT"""
        return hash(self.arg)

    def all_symbols(self):
        """Return set of all symbols in NOT"""
        return self.arg.all_symbols()

class Or(Sentence):
    """
    Logical OR operation
    """
    def __init__(self, *args):
        """Initialize OR with multiple arguments"""
        self.args = list(args)

    def __eq__(self, value):
        """Check if OR sentences are equal"""
        return self.args == value.args and super().__eq__(value)
    
    def evaluate(self, model):
        """Evaluate OR in given model"""
        return any(arg.evaluate(model) for arg in self.args)

    def __str__(self):
        """Return string representation of OR"""
        return "(" + " OR ".join(str(arg) for arg in self.args) + ")"

    def __hash__(self):
        """Return hash value for OR"""
        return hash(tuple(self.args))
    
    def all_symbols(self):
        """Return set of all symbols in OR"""
        return set().union(*(arg.all_symbols() for arg in self.args))


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
        # print(f"Evaluating model: {model}")
        for sentence in self.sentences:
            if not sentence.evaluate(model):
                # print(f"\tDoes not satisfy sentence: {sentence}")
                return False
            # else:
                # print(f"\tSatisfies sentence: {sentence}")
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

    def evaluate_models(self, query):
        """
        Evaluate all models to determine if a query is definitely true, definitely false, or possibly true
        Returns:
            True if the query is definitely true in all valid models
            False if the query is definitely false in all valid models
            None if the query could be true or false (MAYBE)
        """
        valid_models = []
        
        # Collect all valid models
        for model in self.generate_models():
            if self.evaluate(model):
                valid_models.append(model)
        
        # If no valid models, return True (vacuously true)
        if not valid_models:
            return True
            
        # Check if query is true in all valid models
        all_true = True
        all_false = True
        
        for model in valid_models:
            result = query.evaluate(model)
            if result:
                all_false = False
            else:
                all_true = False
        
        if all_true:
            return True  # Definitely true in all models
        elif all_false:
            return False  # Definitely false in all models
        else:
            return None  # Could be true or false (MAYBE)
