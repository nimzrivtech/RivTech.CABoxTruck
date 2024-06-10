export const RoleConstants = {
    System: "System",
    Administrator: "Administrator",
    UnderWriter: "UnderWriter",
    InspectorManager: "InspectorManager",
    Inspector: "Inspector",
    Insured: "Insured",
    AnyoneExceptInsured: [
        "System",
        "Administrator",
        "UnderWriter",
        "InspectorManager",
        "Inspector"
    ],
    Anyone: [
        "System",
        "Administrator",
        "UnderWriter",
        "InspectorManager",
        "Inspector",
        "Insured"
    ]
};