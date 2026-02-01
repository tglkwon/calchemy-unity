---
name: Unity Assembly Guardian
description: Diagnoses and fixes Unity Assembly Definition (.asmdef) errors, specifically missing references and DLL linking issues.
---

# Unity Assembly Guardian 🛡️

Use this skill when you encounter compiler errors related to missing types (`CS0246`), namespaces not found, or issues with Assembly Definition files (`.asmdef`) in Unity.

## 1. Diagnosis Strategy

### 🔍 Symptom: "The type or namespace name 'X' could not be found (CS0246)"
1.  **Locate the Defining File**: Find where class `X` is defined.
    ```bash
    fd -e cs "X.cs"
    ```
2.  **Identify Assemblies**:
    *   Find the `.asmdef` for the **Source** (where `X` is defined).
    *   Find the `.asmdef` for the **Consumer** (where the error is happening).
    *   *Note: If no `.asmdef` is found in a parent folder, it belongs to the `Assembly-CSharp` (Global) assembly.*

### 🛠️ Reference Check
*   If **Source** has an asmdef and **Consumer** has an asmdef:
    *   Open `Consumer.asmdef`.
    *   Check if `Source.asmdef`'s **name** (not filename) is in the `"references"` array.
    *   **Fix**: Add the Source assembly name to `"references"`.

*   If **Source** is in Global (`Assembly-CSharp`) and **Consumer** has an asmdef:
    *   **Error**: Custom assemblies CANNOT reference Component scripts in the default Global assembly.
    *   **Fix**: You MUST create an `.asmdef` for the Source folder (e.g., `LogicForge.Shared.asmdef`) to isolate it, then reference it in the Consumer.

## 2. Handling DLLs & Plugins (Newtonsoft.Json, etc.)

### 🔍 Symptom: "The type or namespace name 'Newtonsoft' could not be found"
*   Even if "Newtonsoft.Json" is in the package list, `.asmdef` files isolate the code from global plugins.

### 🛠️ Configuration Fix
1.  **Check `overrideReferences`**:
    *   Open the failing `.asmdef`.
    *   Look for `"overrideReferences": true`.
2.  **If True (Override Mode)**:
    *   Unity auto-referenced plugins are **IGNORED**.
    *   **Fix**: You must explicitly add the DLL filename (e.g., `Newtonsoft.Json.dll`) to the `"precompiledReferences"` array.
    *   *Do not* add the assembly name to the source `"references"` array if it's a DLL.
3.  **If False (Auto Mode)**:
    *   Ensure the Plugin itself is imported and has "Auto Referenced" checked in its Inspector settings (check `.meta` file if uncertain).

## 3. Best Practices Table

| Scenario | Action |
| :--- | :--- |
| Scripts A needs Scripts B | Add `A.asmdef` -> References -> `B` |
| Scripts A needs Global Scripts | Move Global Scripts to new `C.asmdef`, then `A` refs `C` |
| Scripts A needs Newtonsoft (No asmdef) | Add `Newtonsoft.Json` to `references` (if Auto) OR `precompiledReferences` (if Override) |
| Editor Test needs Runtime Script | `Test.asmdef` -> References -> `Runtime.asmdef` |

## 4. Common Commands

```bash
# Find all asmdef files
fd -e asmdef

# Check assembly definition content
cat path/to/MyAssembly.asmdef
```
