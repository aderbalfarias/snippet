# Azure 

## Resources

- [Installation documentation](http://aka.ms/cli)
- [jq package documentation](https://stedolan.github.io/jq/)
- [JMES path query documentation](http://jmespath.org/)

## Sorted by

1. List CDN commands
2. Create CDN profile
3. Create CDN endpoint
4. List profiles and endpoints
5. Update endpoint
6. Remove profiles and endpoints

## Parsing the JSON output

**Using jq**

```sh
az cdn list | jq '.[].name'
```

**Using `--query`**

```sh
az cdn list --query '[].name'
```

## Looking up valid resource locations

To find valid CDN profile or endpoint resource locations, you can use the `az provider` command and `jq` to query the JSON response.

```sh
# Look up locations for profiles
az provider show --namespace Microsoft.Cdn | \
    jq '.resourceTypes[] | select(.resourceType == "profiles").locations'

# Look up locations for endpoints
az provider show --namespace Microsoft.Cdn | \
    jq '.resourceTypes[] | select(.resourceType == "profiles/endpoints").locations'
```