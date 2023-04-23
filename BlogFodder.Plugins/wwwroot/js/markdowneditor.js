window.easyMDEInstances = {};

window.initializeEasyMDE = (elementId) => {
    window.easyMDEInstances[elementId] = new EasyMDE({element: document.getElementById(elementId)});
};

window.getEasyMDEValue = (elementId) => {
    const easyMDE = window.easyMDEInstances[elementId];
    return easyMDE.value();
};

window.setEasyMDEValue = (elementId, value) => {
    const easyMDE = window.easyMDEInstances[elementId];
    easyMDE.value(value);
};