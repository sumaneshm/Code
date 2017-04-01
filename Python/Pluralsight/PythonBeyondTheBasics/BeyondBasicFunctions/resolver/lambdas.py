from pprint import pprint


def experiment1():
    scientists = ["Marie Curie", "Albert Einstein", "Niels Bohr", "Sumanesh M",
                  "Issac Newton", "Charles Darwin", "Dmitri Mendeleev"]

    pprint(sorted(scientists, key=lambda name: name.split()[-1]))

    last_name = lambda name: name.split()[-1]
    # lambda is a function expression and can be called
    print(last_name("Nikola Tesla"))
    # lambda can have more than one parameters
    adder = lambda a, b: a + b
    print(adder(1, 2))
    # lambda can have zero arguments
    zero = lambda: print("I am Zero")
    zero()
    # body of lambda can only be a single expression
    # lambda can't have return statement


if __name__ == '__main__':
    experiment1()
