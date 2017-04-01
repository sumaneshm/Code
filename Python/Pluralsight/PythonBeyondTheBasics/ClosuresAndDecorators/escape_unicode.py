# decorators are like attributes in C# except that it will change the way how the function behaves
# it accepts a 'callable' and calls it with the parameters passed to it and returns another one "callable"


def convert_number(f):
    # wrap inner function is used to call the function being passed with all the arguments and converts the result of
    # of the function to ascii and returns it
    def wrap(*args, **kwargs):
        r = f(*args, **kwargs)
        if r == 0:
            return "Zero"
        elif r == 1:
            return "One"
        elif r == 2:
            return "Two"
        elif r == 3:
            return "Three"
        elif r == 4:
            return "Four"
        elif r == 5:
            return "Five"
        else:
            return "Greater than 5"

    return wrap


def make_upper(f):
    def wrap(*args, **kwargs):
        r = f(*args, **kwargs)
        if isinstance(r, str):
            return r.upper()
        else:
            return r

    return wrap


# look at how we have used the make_upper as a decorator.
# here we are going to change the number to string automatically
# the main advantage is that the logic is located in a central part
# and we can nest multiple decorators

@make_upper
@convert_number
def adder(a, b):
    return a + b


def experiment1():
    print(adder(3, 2))


if __name__ == '__main__':
    experiment1()
