using System;
using System.Collections.Generic;
using System.Linq;

namespace ZabbixMonitor.Services;

public sealed class NavigationPolicy
{
    private readonly HashSet<string> _allowedHosts;

    public NavigationPolicy(IEnumerable<string> allowedUrls)
    {
        _allowedHosts = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (string url in allowedUrls.Where(x => !string.IsNullOrWhiteSpace(x)))
        {
            if (Uri.TryCreate(url, UriKind.Absolute, out Uri? uri))
            {
                _allowedHosts.Add(uri.Host);
            }
        }
    }

    public bool IsAllowed(string? candidateUri)
    {
        if (string.IsNullOrWhiteSpace(candidateUri))
        {
            return false;
        }

        if (!Uri.TryCreate(candidateUri, UriKind.Absolute, out Uri? uri))
        {
            return false;
        }

        if (uri.Scheme is "about" or "data" or "blob")
        {
            return true;
        }

        if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
        {
            return false;
        }

        return _allowedHosts.Any(allowed =>
            uri.Host.Equals(allowed, StringComparison.OrdinalIgnoreCase) ||
            uri.Host.EndsWith($".{allowed}", StringComparison.OrdinalIgnoreCase));
    }
}
