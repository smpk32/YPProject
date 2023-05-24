mergeInto(LibraryManager.library,{

    Hello: function () {
        //window.alert("Hello, world!");
        console.log("Hello!");
    },

    HelloString: function (str) {
        //window.alert(UTF8ToString(str));
        console.log(UTF8ToString(str));
    },

    QuitGame: function () {
        MoveMain();
    },


})