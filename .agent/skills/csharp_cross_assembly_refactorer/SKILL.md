---
name: C# Cross-Assembly Refactorer
description: Refactors code to resolve cross-assembly constraints, specifically transforming partial classes into extension methods and resolving namespace ambiguities.
---

# C# Cross-Assembly Refactorer 🛠️

Use this skill when you encounter:
1.  **CS0246/CS0436**: Type conflicts or missing definitions due to `partial class` split across assemblies.
2.  **CS1061**: "Does not contain a definition for..." when extensions are not visible.
3.  **CS1929/CS0104**: Ambiguous references (e.g., `UnityEngine.Component` vs `MyNamespace.Component`).

## 1. The "Partial Class" Trap
⚠️ **Rule**: `partial class` definitions MUST reside in the **same assembly**.
You cannot extend a class in `Assets/Shared` (Assembly A) using a partial class in `Assets/Scripts` (Assembly B). This creates two separate types and causes conflicts.

### 🔄 Refactoring Pattern: "Extension Method"
Convert the "Extension" partial class into a Static Class with Extension Methods.

**Before (Invalid Cross-Assembly):**
```csharp
// Assembly: Calchemy.Scripts
namespace LogicForge.Schema {
    public partial class Component { // Error: Creates new Shadow Type
        public void Execute() { ... }
    }
}
```

**After (Valid Pattern):**
```csharp
// Assembly: Calchemy.Scripts
using LogicForge.Schema; 

// 1. Make class static
public static class ComponentExtensions {
    // 2. First parameter is 'this Type'
    public static void Execute(this Component self, Context ctx) {
        // 3. Access public fields via 'self'
        Debug.Log(self.Name); 
    }
}
```

## 2. Resolving Ambiguity (Ambiguous Reference)

### 🔍 Symptom
*   `'Component' is an ambiguous reference between 'UnityEngine.Component' and 'LogicForge.Schema.Component'`
*   `CS1929: 'Component' does not contain a definition for...` (Compiler picked the wrong one)

### 🛠️ Fix Strategy
1.  **Remove `using`**: If possible, remove `using UnityEngine;` if only simple types are needed.
2.  **Alias Directive**:
    ```csharp
    using SchemaComponent = LogicForge.Schema.Component;
    ...
    public static void Execute(this SchemaComponent self) { ... }
    ```
3.  **Fully Qualified Name (Recommended for Extensions)**:
    Use the full path in the method signature to be absolutely clear.
    ```csharp
    public static class ComponentExtensions {
        public static void Execute(this LogicForge.Schema.Component self) { ... }
    }
    ```

## 3. Checklist for Refactoring
- [ ] Determine which Assembly owns the "Core" class.
- [ ] Identify the "Extension" logic file.
- [ ] **Delete** the partial class definition in the Extension file.
- [ ] **Create** a `public static class [Name]Extensions`.
- [ ] **Convert** methods to `public static`.
- [ ] **Add** `this [CoreType] self` as the first parameter.
- [ ] **Update** call sites if necessary (usually not needed, extension methods call syntax `obj.Func()` is compatible).
