class CallCounter:
    def __init__(self, f):
        self.f = f
        self.count = 0

    def __call__(self, *args, **kwargs):
        self.count += 1
        return self.f(*args, **kwargs)


@CallCounter
def countable_function(name):
    print("Hello", name)


def experiment1():
    countable_function("Sumaneh")
    countable_function("Saveetha")
    countable_function("Aadhavan")
    countable_function("Aghilan")
    print("Total number of times this called : ", countable_function.count)


if __name__ == '__main__':
    experiment1()

