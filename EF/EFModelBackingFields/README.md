# NOTE

Among all patterns of blog modeling, the `Blog` and `Blog4` types are preferred, which don't require additional API configuration.

Besides, the attribute `BackingField` seems not working as said by the [document](https://learn.microsoft.com/en-us/ef/core/modeling/backing-field?tabs=data-annotations). The problem effectively deprecates the pattern of `Blog2`.