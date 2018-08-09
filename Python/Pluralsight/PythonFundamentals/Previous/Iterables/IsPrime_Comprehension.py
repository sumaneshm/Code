from math import sqrt


def is_prime(x):
    if x < 2:
        return False

    for i in range(2, int(sqrt(x)) + 1):
        if x % i == 0:
            return False

    return True

all_primes_within_101 = [i for i in range(101) if is_prime(i)]

print(all_primes_within_101)
