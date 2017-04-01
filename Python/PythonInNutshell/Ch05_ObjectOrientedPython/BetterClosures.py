__author__ = 'sumaneshm'

#Simple closures

def make_adder(augend):
    def add(addend):
        return addend + augend
    return add

ex1 = make_adder(10)
print("Example 1 : Simple Closure : ", ex1(20))

# Bound method

def make_adder_as_bound_method(augend):
    class Adder(object):
        def __init__(self, augend): self.augend = augend
        def add(self, addend): return self.augend + addend
    return Adder(augend).add

ex2 = make_adder_as_bound_method(10)
print("Example 2 : Bound method : ", ex2(20))

# Bound method with callable instance

def make_adder_as_callable_instance(augend):
    class Adder(object):
        def __init__(self, augend): self.augend = augend
        def __call__(self, addend): return self.augend + addend
    return Adder(augend)

ex3 = make_adder_as_bound_method(10)
print("Example 3 : Bound method : ", ex3(20))

def make_adder_as_callable_instance(augend):
    class Adder(object):
        def __init__(self, augend): self.augend = augend
        def __call__(self, addend): return self.augend + addend
