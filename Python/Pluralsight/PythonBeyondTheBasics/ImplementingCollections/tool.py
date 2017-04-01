import inspect
import itertools
from pprint import pprint as pp
import sys

def underline(text):
    print("\n" + text)
    print("=" * len(text))


def print_table(rows_of_columns, *headers):
    num_columns = len(rows_of_columns[0])
    num_headers = len(headers)

    if num_columns != num_headers:
        raise TypeError("Expected {} number of columns, but got {}".format(num_headers, num_columns))

    rows_of_columns_with_headers = itertools.chain([headers], rows_of_columns)
    columns_of_rows = list(zip(*rows_of_columns_with_headers))
    column_widths = [max(map(len, column)) for column in columns_of_rows]
    # pp(column_widths)
    # return
    column_specs = ('{{:{w}}}'.format(w=width) for width in column_widths)
    format_specs = ' '.join(column_specs)
    header_row = format_specs.format(*headers)
    print(header_row)
    print('-' * len(header_row))
    for row in rows_of_columns:
        print(format_specs.format(*row))



def full_sig(m):
    try:
        return m.__name__ + str(inspect.signature(m))
    except Exception as e:
        return m.__name__ + "(...)"


def brief_doc(m):
    doc = m.__doc__
    if doc is not None:
        lines = doc.splitlines()
        if len(lines) > 0:
            return lines[0]
    return ''


def dump(obj):
    underline("Text")
    print(type(obj))

    underline("Documentation")
    print(inspect.getdoc(obj))          # inspect.getdoc => gets cleaned doc from the object passed

    all_attr_names = dir(obj)
    method_names = list(filter(lambda attr_name: callable(getattr(obj, attr_name)), all_attr_names))
    attr_names = [name for name in all_attr_names if name not in method_names]
    attr_name_and_value = [(name, repr(getattr(obj, name)))
                                    for name in attr_names]

    underline("Attributes")
    print_table(attr_name_and_value, "Name", "Value")

    underline("Method")
    methods = [getattr(obj, name) for name in method_names]
    methods_names_and_doc = [(full_sig(m), brief_doc(m))
                             for m in methods]
    print_table(methods_names_and_doc, "Name", "Brief doc")


if __name__ == '__main__':
    dump("")