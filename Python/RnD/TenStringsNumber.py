def sum_digits(n):
    """
    sums all the individual digits
    :param n:
    number
    :return:
    sum of all the individual digits
    """
    s = 0
    sn = str(n)

    for d in sn:
        s += int(d)

    return s


def is_perfect_ten_string_number(n):
    """
    Checks whether the given number is a perfect ten string number or not
        Ten string number : when we add all the digits, will give ten
        Perfect ten string number : when all the digits in a number are used for forming a "ten string number"

    :param n:
        number to check
    :return:
        boolean to indicate whether it is a perfect ten string number
    """
    if n < 10:
        return False

    start_counter = 0

    str_n = str(n)

    not_considered_string_positions = set(range(0, len(str_n)))  # {i for i in range(0, len(str_n)) if str_n[i] != "0"}

    while start_counter < len(str(n)) and len(not_considered_string_positions) > 1:
        sn = str(n)[start_counter:]
        counter = 0
        buffer = ""
        sum_buffer = 0

        while counter < len(sn):
            buffer += sn[counter]
            sum_buffer = sum_digits(buffer)

            if sum_buffer >= 10:
                break
            counter += 1

        if sum_buffer == 10 or sum_buffer == 0:
            for i in range(0, counter + 1):
                not_considered_string_positions.discard(start_counter + i)
        else:
            if not_considered_string_positions.__contains__(start_counter) and str_n[start_counter] != "0":
                if sum_buffer > 10:
                    minpos = min(not_considered_string_positions)
                    n1 = int(str_n[minpos]) + 1
                    n2 = len(str_n) - minpos - 1
                    nexti = (n1) * (10 ** n2)

                    return nexti, False
                else:
                    return n + 1, False

        start_counter += 1
    return n + 1, True


def get_next_i(str_n, pos):
    strleftn = str_n[:pos+1]
    if strleftn == "":
        leftn = 0
    else:
        leftn = int(strleftn) + 1
    rightn = "0" * (len(str_n) - pos-1)
    nexti = str(leftn) + str(rightn)
    return int(nexti)


def test_get_next_i():
    expt = {
        56: 60,
        156: 160,
        1156: 1160,
        11156: 11160,
        111156: 111160
    }

    pos = 0
    for i in expt:
        next_i = get_next_i(str(i), pos)
        pos += 1


def test_is_perfect_ten_string_number():
    """
    unit test for sum_digits
    :return:
    none
    """

    test_data = {
        19: True,
        190: True,
        1928: True,
        3523014: True,
        98021: False,
        87375: False,
        72948: False,
        287: False
    }

    for i in test_data:
        pos, res = is_perfect_ten_string_number(i)

        if test_data[i] != res:
            print("Expectation mismatch for ", i)
    print("Completed")


def count_perfect_ten_strings_under(pown):
    n = 10 ** pown
    i = 10
    counter = 0

    while i < n:

        if i % 10000 == 0:
            print(n - i, " to go...")

        # if sum_digits(stri[:2]) > 10:
        #    n1 = int(stri[0]) + 1
        #    n2 = len(stri) - 1
        #    i = (n1) * (10 ** n2)

        i, result = is_perfect_ten_string_number(i)
        if result:
            # print ("Perfect : " , i)
            counter += 1

    return counter


if __name__ == "__main__":
    test_get_next_i()
    # print("Count of perfect ten strings : ", count_perfect_ten_strings_under(5))
    # print(is_perfect_ten_string_number(356))
    # test_is_perfect_ten_string_number()
