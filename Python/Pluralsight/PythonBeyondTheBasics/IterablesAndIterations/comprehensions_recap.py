
def experiment1():
    l = [i * 2 for i in range(10)]  # list comprehensions
    print(l)

    s = {i for i in range(10)}  # set comprehensions
    print(s)

    d = {i: i*2 for i in range(5)}  # dictionary comprehensions
    print(d)

    g = (i for i in range(5)) # generator comprehensions
    print(list(g))

    # dual comprehension
    # i is outer, j is inner
    l2 = [(i, j) for i in range(5) for j in range(5)]
    print(l2)


def experiment2():
    # we can have if statements and nested comprehensions
    # we can refer to any variables declared before (note the use of i in side j loop)
    l1 = [(i, j) for i in range(10) if i > 5 for j in range(i, 10)]

    # this is syntactically equivalent to the following:
    l2 = []
    for i in range(10):
        if i > 5:
            for j in range(i, 10):
                l2.append((i, j))

    print(l2 == l1)


def experiment3():
    # we can nest comprehensions inside another comprehension
    # notice how i is available inside the inner comprehension
    nested1 = {i: [j for j in range(i)] for i in range(5)}
    print(nested1)

    # syntactic equivalent would look like
    nested2 = {}
    for i in range(5):
        inner = []
        for j in range(i):
            inner.append(j)
        nested2[i] = inner
    print(nested2)

    print(nested1 == nested2)



if __name__ == '__main__':
    experiment1()
