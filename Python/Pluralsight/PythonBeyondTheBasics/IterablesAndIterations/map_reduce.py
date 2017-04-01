from pprint import pprint
from functools import reduce


def count_words(doc):
    frequencies = {}
    for word in doc.split():
        frequencies[word] = frequencies.get(word, 0) + 1
    return frequencies


def combine_words_count_dict(d1, d2):
    d = d1.copy()
    for word, count in d2.items():
        d[word] = d.get(word, 0) + count
    return d


def magic_map_reduce():
    docs = [
        "This is document1 and I am impressed",
        "This is document2 and I am really really impressed",
        "This is document3 by such a cool language"
    ]

    counts = map(count_words, docs)
    combined_dict = reduce(combine_words_count_dict, counts)
    pprint(combined_dict)

if __name__ == '__main__':
    magic_map_reduce()
