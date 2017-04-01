
def check_non_negative(index):
    def validator(f):
        # wrap method creates two closures [index, f]
        def wrap(*args, **kwargs):
            if args[index] < 0:
                message = "Argument {} can't be negative".format(index)
                raise ValueError(message)
            return f(*args, **kwargs)
        return wrap
    return validator


# Python will intelligently "call" the method first and use the return as a decorator
@check_non_negative(1)
def two_artument_method(any, non_negative):
    print("Any : {}, Non-negative : {}".format(any, non_negative))


def experiment1():
    two_artument_method(2, 10)
    # this call would obviously fail
    # two_artument_method(1, -10)


if __name__ == '__main__':
    experiment1()