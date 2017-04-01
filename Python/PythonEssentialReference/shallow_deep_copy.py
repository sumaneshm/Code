import copy


def experiment1():
    a = [1, 2, [100, 200], 300]
    l = list(a) # this is shallow copy
    print(a is l)
    l.append(500)
    print(l) # only l is impacted
    print(a) # a is not impacted as we are amending the list
    l[2][0] = "Sumanesh"
    # here both l[2] and a[2] are pointing to the same item and hence changing l[2] will impact a[2]
    print(l)
    print(a)


def experiment2():
    a = [1, 2, [100, 200], 300]
    l = copy.deepcopy(a)
    l[2][0] = "Sumanesh"
    # here both l[2] and a[2] are pointing to the different items and hence changing l[2] will NOT impact a[2]
    print(l)
    print(a)


if __name__ == '__main__':
    experiment2()