import socket


class Resolver:

    def __init__(self):
        self._cache = {}

    # this special method makes the instance of "Resolver" as callable".
    # that means, it can be called directly by using the instance name as shown in the example
    # i.e. makes the instance act as a function
    # this will be used when the functions need to have "state"
    def __call__(self, host):
        if host not in self._cache:
            print("Adding the host {} to the cache".format(host))
            self._cache[host] = socket.gethostbyname(host)
        else:
            print("Returning IP info for {} from the cache".format(host))

        return self._cache[host]


    #function class can perfectly have methods as well
    def clear(self):
        self._cache.clear()


    def has_host(self, host):
        return host in self._cache


resolver = Resolver()

def experiment1():
    # observe how we call the __call__ special method by using the instance
    # name directly
    print(resolver("google.com"))
    # it is just a sugarcoat for calling the method like
    print(resolver.__call__("google.com"))
    # we can also inspect the state of the cache
    print(resolver._cache)


    # we will use timer to check
    from timeit import timeit

    print(timeit(setup="from resolver import resolver",   # what to get?
                 stmt="resolver('python.org')",            # what to do?
                 number=1))                               # how many times?

    print("%f" % timeit(setup="from resolver import resolver",   # what to get?
                 stmt="resolver('python.org')",            # what to do?
                 number=1))                               # how many times?


# methods can contain
def experiment2():
    def give_class(immutable):
        # class can be passed back and used dynamically
        # this is a conditional expression

        return set if immutable else list

    print(give_class(True))
    print(give_class(False))

if __name__ == '__main__':
    experiment2()
