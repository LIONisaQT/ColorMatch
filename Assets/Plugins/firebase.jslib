mergeInto(LibraryManager.library, {
  GetJSON: function (path, objectName, callback, fallback) {
    var parsedPath = Pointer_stringify(path);
		var parsedObjectName = Pointer_stringify(objectName);
		var parsedCallback = Pointer_stringify(callback);
		var parsedFallback = Pointer_stringify(fallback);

		console.log(window.db);
		console.log(window.test);

		try {
			const querySnapshot = window.getData();
			window.unityInstance.Module.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(querySnapshot));
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

		console.log(window.db);
		console.log(window.test);
		console.log(parsedPayload);

		try {
			window.sendData(parsedPayload);
			window.unityInstance.Module.SendMessage(parsedObjectName, parsedCallback, "Payload sent!");
		} catch (error) {
			window.unityInstance.Module.SendMessage(parsedObjectName, parsedFallback, "There was an error: " + error.message);
		}
	}
});