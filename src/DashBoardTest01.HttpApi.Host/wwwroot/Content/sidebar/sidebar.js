window.dxDemo = window.dxDemo || {};


dxDemo.Sidebar = {
    viewModel: null,
    initPopup: function(demoName, platform) {
        dxDemo.Sidebar.viewModel = {            
            title: ko.observable(""),
            popupVisible: ko.observable(false),
            description: ko.observable([]),
            links: ko.observable([]),
            isMobileView: ko.observable(DevExpress.devices.current().phone),
            feedback: dxDemo.Feedback && dxDemo.Feedback.getKoViewModel(),
            demoName: demoName,
            platform: ko.observable(platform)
        };

        ko.applyBindings(dxDemo.Sidebar.viewModel, document.getElementById("demo-info-sidebar"));
    },
    showDemoPopup: function (dashboardControl, dashboardModel, tag) {
        var options = dxDemo.Sidebar.getDataFromCurrentDashboard(dashboardControl, dashboardModel),
            viewModel = dxDemo.Sidebar.viewModel;
        viewModel.description(options.description);
        viewModel.links(options.links);
        viewModel.title(options.title + " Dashboard");
            
        viewModel.popupVisible(true);

        dxDemo.Sidebar.trackGAEvent(viewModel.demoName, "Open Sidebar", "v2" + (tag ? " - " + tag : ""));
    },
    hideDemoPopup: function () {
        dxDemo.Sidebar.viewModel.popupVisible(false);
    },
    getDataFromCurrentDashboard: function (dashboardControl, dashboardModel) {
        var options = {
            description: JSON.parse(dashboardModel.customProperties.getValue("Description") || "[]"),
            links: JSON.parse(dashboardModel.customProperties.getValue("Links") || "[]"),
            title: dashboardModel.title.text(),
            id: dashboardControl && dashboardControl.dashboardContainer().id
        };
        return options;
    },
    getToolbarItem: function(dashboardControl) {
        return {
            icon: "dx-dashboard-data-reduced",
            type: "button",
            hint: "Show Info Panel",
            click: function() {
                dxDemo.Sidebar.showDemoPopup(dashboardControl, dashboardControl.dashboard(), "toolbar");
            }
        };
    },

    trackGAEvent: function(category, action, label) {
        if(window.ga && window.ga.getAll) {
            var _tracker = ga.getAll()[0];
            if(_tracker) _tracker.send('event', category, action, label);
        }
    }
};