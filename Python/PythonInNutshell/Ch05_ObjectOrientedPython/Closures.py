__author__ = 'sumaneshm'

# Closure is good for simple cases, however Bound methods are more powerful.
# refer to the BetterClosures.py for example

def make_adder(augend):
    def add(addend):
        return addend + augend #Referencing augend (outer variable inside another one function is called as closure)
    return add

a = make_adder(10)
print(a(20))
