mergeInto(LibraryManager.library, {
  GetJSON: function (path, objectName, callback, fallback) {
    var parsedPath = Pointer_stringify(path);
		var parsedObjectName = Pointer_stringify(objectName);
		var parsedCallback = Pointer_stringify(callback);
		var parsedFallback = Pointer_stringify(fallback);

		try {
			window.unityInstance.Module.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(window.playerData));
		} catch (error) {
			window.unityInstance.Module.SendMessage(parsedObjectName, parsedFallback, "There was an error: " + error.message);
		}
  },

	SendJSON: function (path, objectName, payload, callback, fallback) {
		var parsedPath = Pointer_stringify(path);
		var parsedObjectName = Pointer_stringify(objectName);
		var parsedPayload = Pointer_stringify(payload);
		var parsedCallback = Pointer_stringify(callback);
		var parsedFallback = Pointer_stringify(fallback);

		try {
			window.sendData(parsedPayload);
			window.unityInstance.Module.SendMessage(parsedObjectName, parsedCallback, "Payload sent!");
		} catch (error) {
			window.unityInstance.Module.SendMessage(parsedObjectName, parsedFallback, "There was an error: " + error.message);
		}
	}
});