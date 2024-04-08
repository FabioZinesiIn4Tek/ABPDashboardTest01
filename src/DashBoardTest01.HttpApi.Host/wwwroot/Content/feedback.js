window.dxDemo = window.dxDemo || {};

dxDemo.Feedback = (function() {
    var _getCookieValue = function(name) {
        var match = document.cookie.match('(^|;)\\s*' + name + '\\s*=\\s*([^;]+)');
        return match ? match.pop() : '';
    };
    var baseUrl = "",
        baseDemoName = "";

    var UiMachine = function() {
        var state = {
            dto: null,
            moduleName: ""
        };
        return {
            stateChanged: function() { },
            init: function(moduleName) {
                if(state.moduleName === moduleName) {
                    return;
                }
                state.moduleName = moduleName;
                state.dto = null;
                this.stateChanged('default');
            },
            sendFeedback: function(positive) {
                this.stateChanged('sendingFeedback');
                var that = this;
                feedbackObject.post({
                    moduleName: state.moduleName,
                    positive: positive
                }).then(function(dto) {
                    state.dto = dto;
                    that.stateChanged('feedbackSent', {
                        positive: positive,
                        thankYouText: positive ? "Thank you for your positive response!": "Thank you for your feedback!",
                        invitationText: positive ? "You can share your ideas or close the dialog. We welcome any feedback." : "Please tell us how you'd like to improve this demo or close the dialog."
                    });
                    }, function () { that.stateChanged('failure'); });
            },
            sendComment: function(comment) {
                var that = this;
                if(state.dto) {
                    this.stateChanged('sendingComment');
                    feedbackObject
                        .put(comment, state.dto)
                        .then(function() {
                            that.stateChanged('commentSent');
                        }, function() { that.stateChanged('failure'); });
                }
            },
            closeComments: function() {
                this.stateChanged('commentSent');
            }
        };
    };


    var feedbackObject = {
        init: function(url, baseName) {
            baseUrl = url;
            baseDemoName = baseName;
        },

        composeModuleName: function(moduleName) {
            var baseName = baseDemoName + ".",
                version = ".v" + (DevExpress.VERSION || /Version='(.[0-9.]*)'/.exec(ASPx && ASPx.VersionInfo)[1]);
            if(baseName.length + version.length + moduleName.length > 100) {
                moduleName = moduleName.substring(0, 100 - baseName.length - version.length - 1) + "…";
            }
            return baseName + moduleName + version;
        },

        post: function(args) {
            var realVisitorId = _getCookieValue("DXVisitor"),
                moduleName = this.composeModuleName(args.moduleName),
                visitorId = realVisitorId ? "{" + realVisitorId + "}" : "{34FE55BA-2FAF-4C19-AFDE-190B3D2EAF5D}",
                value = args.positive ? "1" : "-1";
            return $.ajax({
                type: 'POST',
                url: baseUrl + "/api/v1/feedback",
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({
                    moduleName: moduleName,
                    userId: visitorId,
                    feedback: args.feedback,
                    value: value
                })
            }).then(function(r) {
                return {
                    id: r.id,
                    moduleName: moduleName,
                    userId: visitorId,
                    value: value
                };
            });
        },

        put: function(feedbackComment, args) {
            args.feedback = feedbackComment;
            return $.ajax({
                type: 'PUT',
                url: baseUrl + "/api/v1/feedback",
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify(args)
            });
        },
        getStateMachine: function() {
            return new UiMachine();
        },
        getKoViewModel: function() {
            var machine = this.getStateMachine();
            machine.stateChanged = function(state, args) {
                switch(state) {
                    case "default":
                        vm.canSendFeedback(true);
                        vm.feedbackIsSent(false);
                        vm.commentSectionVisible(false);
                        vm.increasedSpace(false);
                        vm.feedbackText("");
                        vm.thankYouText("");
                        vm.isClosed(false);
                        vm.confirmationVisible(false);
                        break;
                    case "sendingFeedback":
                        vm.showLoading(true);
                        vm.canSendFeedback(false);
                        break;
                    case "feedbackSent":
                        vm.increasedSpace(true);
                        vm.showLoading(false);
                        vm.feedbackIsSent(false);
                        vm.commentSectionVisible(false);

                        vm.confirmationVisible(true);


                        setTimeout(function() {
                            vm.confirmationVisible(false);

                            vm.feedbackIsSent(true);
                            vm.commentInvitationText(args.invitationText);
                            vm.thankYouText(args.thankYouText);
                            vm.commentSectionVisible(true);                            
                        }, 1500);
                        
                        vm.hasFocus(true);
                        break;
                    case "sendingComment":
                        vm.showLoading(true);
                        vm.commentSectionVisible(false);
                        break;
                    case "commentSent":
                        vm.showLoading(false);
                        vm.commentSectionVisible(false);
                        setTimeout(function() {
                            vm.isClosed(true);
                        }, 1000);
                        break;
                    case "failure":
                        vm.feedbackIsSent(true);
                        vm.canSendFeedback(false);
                        vm.showLoading(false);
                        vm.commentSectionVisible(false);
                        break;
                }
            };
            var vm = {
                canSendFeedback: ko.observable(true),
                feedbackIsSent: ko.observable(false),
                commentSectionVisible: ko.observable(false),
                increasedSpace: ko.observable(false),
                commentInvitationText: ko.observable(""),
                thankYouText: ko.observable(""),
                feedbackText: ko.observable(""),
                showLoading: ko.observable(false),
                isClosed: ko.observable(false),
                hasFocus: ko.observable(false),
                confirmationVisible: ko.observable(false),
                sendPositiveFeedback: function() {
                    machine.sendFeedback(true);
                },
                sendNegativeFeedback: function() {
                    machine.sendFeedback(false);                    
                },
                closeComments: function() {
                    machine.closeComments();
                },
                sendComment: function(vm) {
                    if(vm.feedbackText().length) {
                        machine.sendComment(vm.feedbackText());
                    }
                },
                init: function(moduleName) {
                    machine.init(moduleName);                    
                }
            };
            return vm;
        }
    };

    return feedbackObject;
})();