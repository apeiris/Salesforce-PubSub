﻿
## 1. Enable OAuth Settings in Salesforce
Ensure the **OAuth and OpenID Connect Settings** are configured:

- Go to **Setup > Security > OAuth and OpenID Connect Settings**.
- Enable the <span style="color: green; font-weight: bold; font-style: italic;">Allow OAuth Username-Password Flows</span> option if using the Username-Password Flow (temporary workaround; see [Security Note](#security-note)).
The link is available in the **OAuth and OpenId Connect (for DEVHUB)** settings. (eg. `https://irissystems-dev-ed.lightning.force.com/lightning/setup/OauthOidcSettings/home`))
[OAuth and OpenID Connect Settings](https://irissystems-dev-ed.lightning.force.com/lightning/setup/OauthOidcSettings/home)

## 2. Access Login History for Diagnostics
Monitor login attempts for troubleshooting:
- Navigate to **Profile > My Personal Information > Login History** (<span style="color: red;">User Menu > My Settings > Login History</span>).


---


### Security Note
The **Username-Password Flow** is deprecated and less secure. Consider migrating to the **Web Server Flow** or **JWT Bearer Flow** for production use. Update the Connected App and code accordingly to avoid future compatibility issues.
