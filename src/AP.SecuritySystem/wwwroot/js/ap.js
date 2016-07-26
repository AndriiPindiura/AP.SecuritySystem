var ap = {
    selectedId: {},
    partialViewComponent: {},
    partialParameter: {},
    partialCaption: {},
    partialIcon: {},
    partialUrl: {},
    serverId: {},
    SetPartialInfo: function (element) {
        ap.partialViewComponent = element.getAttribute('name');
        ap.partialParameter = element.getAttribute('data-target');
        for (var i = 0; i < element.childNodes.length; i++) {
            if (element.childNodes[i].nodeType == 1) {
                if (element.childNodes[i].getAttribute('class').indexOf('title') > -1) {
                    ap.partialCaption = element.childNodes[i].innerHTML;
                }
                if (element.childNodes[i].getAttribute('class').indexOf(' icon') > -1) {
                    ap.partialIcon = element.childNodes[i].getAttribute('class').replace(' icon', '');
                }
            }
        }
        ap.partialUrl = $('#sidebarUrl').val().replace('view', ap.partialViewComponent).replace('param', ap.partialParameter).replace('caption', escape(ap.ToPascalCase(ap.partialCaption))).replace('iconimage', ap.partialIcon);
    },
    HtmlToDiv: function () {
        $("#divResult").html('<div class="preloader-ring  dark-style" style="margin-top:25%; margin-left: 45%" data-role="preloader" data-type="ring"></div>');

        //alert(ap.partialUrl);
        $.ajaxSetup({
            // Disable caching of AJAX responses
            cache: false
        });
        $("#divResult").load(ap.partialUrl);
    },
    Drop: function (ev) {
        ev.preventDefault();
        var data = ev.dataTransfer.getData('text').split("|")[0];
        var type = ev.dataTransfer.getData('text').split("|")[1];
        //alert(type);
        //alert(data);
        //alert('TARGET: ' + ev.target.getAttribute('class'));
        if (ev.target.getAttribute('data-type') == type) {
            if (ev.target.getAttribute('class').indexOf('listview') > -1) {
                ev.target.appendChild(document.getElementById(data));
            }
            else if (ev.target.getAttribute('class').indexOf('list') > -1) {
                ev.target.parentElement.appendChild(document.getElementById(data));

                //alert(ev.target.parentElement.getAttribute('class'));
                //alert(ev.target.parent().getAttribute('class'));
            }
        }
    },
    Drag: function (ev) {
        ev.dataTransfer.setData("text", ev.target.id + "|" + ev.target.getAttribute('data-type'));
    },
    AllowDrop: function (ev) {
        ev.preventDefault();
    },
    ToPascalCase: function toTitleCase(str) {
        return str.replace(/\w\S*/g, function (txt) { return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase(); });
    },
    PushMessage: function (type, message, keep) {
        var mes = message;
        $.Notify({
            caption: mes.split("|")[0],
            content: mes.split("|")[1],
            type: type,
            keepOpen: keep
        });
    }

};

function pushMessage(type, message) {
    var mes = message;
    $.Notify({
        caption: mes.split("|")[0],
        content: mes.split("|")[1],
        type: t
    });
}

function getIdsInDiv(element) {
    var ids = new Array();
    for (var i = 0; i < element.childNodes.length; i++) {
        if (element.childNodes[i].nodeType == 1) {
            if (element.childNodes[i].getAttribute('class').indexOf('list') > -1) {
                ids.push(element.childNodes[i].getAttribute('id'));
            }
        }
    }
    if (ids.length == 0) {
        ids.push('');
    }
    return ids;
}

function pushError(message) {
    $.Notify({
        caption: message.split("|")[0],
        content: message.split("|")[1],
        type: "alert"
    });
}

$(window).load(function () {
    ap.selectedId = null;
    ap.serverId = -1;
    var form = $(".login-form");
    form.css({
        opacity: 1,
        "-webkit-transform": "scale(1)",
        "transform": "scale(1)",
        "-webkit-transition": ".5s",
        "transition": ".5s"
    });

    $(document).on('click', '.list-content', function () {
        alert('list click');
        ap.selectedId = this.getAttribute('id');
    });

    // Sidebar clicked
    $('.sidebar li').on('click', 'a', function () {
        var icon, caption;
        //$("#divResult").html('<div class="preloader-square  color-style" style="margin-top:25%; margin-left: 45%" data-role="preloader" data-type="square"></div>');

        //alert($("input#qwerty").val());
        ap.SetPartialInfo(this);
        ap.HtmlToDiv();
    });

    // Sidebar navigation Active Class
    $('.sidebar').on('click', 'li', function () {
        if (!$(this).hasClass('active')) {
            $('.sidebar li').removeClass('active');
            $(this).addClass('active');
        }
    });

    // Submit Create User From
    $(document).on('submit', '#createUserForm', function (e) {
        e.preventDefault();

        if ($('#divConfirmPassword').hasClass('error')) {
            ap.PushMessage('alert', 'Create user|Password empty or does not match!', true);
        }
        else {
            //var assignedRoles = new Array();
            //assignedRoles = getIdsInDiv(document.getElementById('assigned'));
            //alert(assignedRoles.length);
            var editPost = {
                Username: document.getElementById('inputUsername').value,
                Email: document.getElementById('inputEmail').value,
                Password: document.getElementById('inputPassword').value,
                AssignedRoles: getIdsInDiv(document.getElementById('assigned'))
            };
            var data = $(this).serialize(),
                action = $(this).attr("action"),
                method = $(this).attr("method");
            //alert(data);
            $.ajax({
                url: action,
                type: method,
                data: editPost,
                dataType: "json",
                success: function (data) {
                    //alert(data.Result);
                    if (data.Result) {
                        //alert(data.Result);
                        ap.PushMessage('success', 'User create|User successfully created', false);
                        ap.HtmlToDiv();
                    }
                    else {
                        var errors = '';
                        data.Errors.forEach(function (error) {
                            errors = errors.concat(error.Description);
                        });
                        //alert(errors);
                        ap.PushMessage('alert', 'User create|'.concat(errors), false);

                    }

                },
                error: function (ex, error) {
                    alert("error: " + ex + error);
                    ap.HtmlToDiv();

                },
                complete: function () {
                }
            });
        }

    });

    // Submit Edit User Form
    $(document).on('submit', '#editUserForm', function (e) {
        e.preventDefault();
        var id = $('#userId').val();
        var email = $('#email').val();
        var assignedRoles = new Array();
        assignedRoles = getIdsInDiv(document.getElementById('assigned'));
        var editPost = {
            Id: id,
            Email: email,
            AssignedRoles: assignedRoles
        }

        var data = $(this).serialize(),
            action = $(this).attr("action"),
            method = $(this).attr("method");
        $.ajax({
            url: action,
            type: method,
            data: editPost,
            success: function (data) {
                if (data.Result) {
                    ap.PushMessage('success', 'User edit|User successfully changed', false);
                    ap.HtmlToDiv();
                }
                else {
                    var errors = '';
                    data.Errors.forEach(function (error) {
                        errors = errors.concat(error.Description);
                    });
                    ap.PushMessage('alert', 'User edit|'.concat(errors), false);
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("error: " + textStatus + "\r\n" + errorThrown + "\r\n" + XMLHttpRequest);
            },
            complete: function () {
            }
        });


    });

    // Submit Role Form
    $(document).on('submit', '#editRoleForm', function (e) {
        e.preventDefault();
        var editPost = {
            Id: $(this).attr('data-roleId'),
            Name: $('#roleName').val(),
            AssignedUsers: getIdsInDiv(document.getElementById('assignedUsers')),
            AssignedServers: getIdsInDiv(document.getElementById('assignedServers'))
        };
        //alert(editPost.AssignedEntries.length);
        var action = $(this).attr("action"),
            method = $(this).attr("method");

        $.ajax({
            url: action,
            type: method,
            data: editPost,
            success: function (data) {
                if (data.Result) {
                    ap.PushMessage('success', 'Role edit|Role successfully changed', false);
                    ap.HtmlToDiv();
                }
                else {
                    var errors = '';
                    data.Errors.forEach(function (error) {
                        errors = errors.concat(error.Description);
                    });
                    ap.PushMessage('alert', 'Role edit|'.concat(errors), false);
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("error: " + textStatus + "\r\n" + errorThrown + "\r\n" + XMLHttpRequest);
            },
            complete: function () {
            }
        });


    });

    // Submit Server Form
    $(document).on('submit', '#editServerForm', function (e) {
        e.preventDefault();
        var serverType = document.getElementById('selectServertype');
        //alert($(this).attr('data-serverId'));
        if (serverType.selectedIndex != -1) {
            //Если есть выбранный элемент, отобразить его значение (свойство value)
            //alert(serverType.options[serverType.selectedIndex].value);
        }

        var server = {
            Id: $(this).attr('data-serverId'),
            Description: $('#inputServername').val(),
            Ip: $('#inputIp').val(),
            Mode: serverType.options[serverType.selectedIndex].value,
            ConnectionString: $('#inputConnectionstring').val(),
            ItvName: $('#inputITVname').val()
        }
        //alert('Server Model assigned');
        var editPost = {
            Server: server,
            AssignedRoles: getIdsInDiv(document.getElementById('assignedRoles'))
        };
        //alert('Edit Model assigned');
        //alert(editPost.AssignedEntries.length);
        var action = $(this).attr("action"),
            method = $(this).attr("method");
        //alert(action);
        $.ajax({
            url: action,
            type: method,
            data: editPost,
            success: function (data) {
                if (data.Result) {
                    ap.PushMessage('success', 'Server edit|Server successfully changed', false);
                    ap.HtmlToDiv();
                }
                else {
                    var errors = '';
                    data.Errors.forEach(function (error) {
                        errors = errors.concat(error.Description);
                    });
                    ap.PushMessage('alert', 'Server edit|'.concat(errors), false);
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("error: " + textStatus + "\r\n" + errorThrown + "\r\n" + XMLHttpRequest);
            },
            complete: function () {
            }
        });


    });

    // Submit Entry Form
    $(document).on('submit', '#editEntryForm', function (e) {
        e.preventDefault();
        var serverSelector = document.getElementById('selectServer');
        var rayTypeSelector = document.getElementById('selectType');
        var rayType = (rayTypeSelector.options[rayTypeSelector.selectedIndex].value === "true");
        //alert(rayType);
        //alert($(this).attr('data-serverId'));
        var entry = {
            Id: $(this).attr('data-entryId'),
            ServerId: serverSelector.options[serverSelector.selectedIndex].value,
            NoE: $('#inputNoe').val(),
            RaysType: rayType,
            Description: $('#inputEntryname').val(),
            EnterRay: $('#inputEnterray').val(),
            ExitRay: $('#inputExitray').val(),
            EnterCamera: $('#inputEnterCam').val(),
            ExitCamera: $('#inputExitCam').val(),
            UpCamera: $('#inputUpCam').val(),
            EnterDelay: $('#inputEnterDelay').val(),
            ExitDelay: $('#inputExitDelay').val()
        }
        var action = $(this).attr("action"),
            method = $(this).attr("method");
        //alert(action);
        $.ajax({
            url: action,
            type: method,
            data: entry,
            success: function (data) {
                if (data.Result) {
                    ap.PushMessage('success', 'Entry edit|Entry successfully changed', false);
                    ap.HtmlToDiv();
                }
                else {
                    var errors = '';
                    data.Errors.forEach(function (error) {
                        errors = errors.concat(error.Description);
                    });
                    ap.PushMessage('alert', 'Entry edit|'.concat(errors), false);
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("error: " + textStatus + "\r\n" + errorThrown + "\r\n" + XMLHttpRequest);
            },
            complete: function () {
            }
        });


    });

    // Submit Change Password Form
    $(document).on('submit', '#changePasswordForm', function (e) {
        e.preventDefault();
        if ($('#divConfirmPassword').hasClass('error')) {
            ap.PushMessage('alert', 'Change password|Password empty or does not match!', true);
        }
        else {

            var userId = $('#userId').val(),
                userToken = $('#token').val(),
                password = $('#inputConfirmPassword').val();
            var action = $(this).attr("action"),
                method = $(this).attr("method");
            var editPost = {
                id: userId,
                token: userToken,
                password: password
            }

            $.ajax({
                url: action,
                type: method,
                data: editPost,
                success: function (data) {
                    if (data.Result) {
                        ap.PushMessage('success', 'Change password|Password successfully changed', false);

                        window.location.href = window.location.origin;
                        //ap.HtmlToDiv();
                    }
                    else {
                        var errors = '';
                        data.Errors.forEach(function (error) {
                            errors = errors.concat(error.Description);
                        });
                        ap.PushMessage('alert', 'User edit|'.concat(errors), false);
                    }

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("error: " + textStatus + "\r\n" + errorThrown + "\r\n" + XMLHttpRequest);
                },
                complete: function () {
                }
            });
        }


    });

    // Password confirm on Keyup
    $(document).on('keyup', '#inputConfirmPassword', function () {
        var inputPassword = document.getElementById('inputPassword');
        var inputConfirmPassword = document.getElementById('inputConfirmPassword');
        var informerConfirmPassowrd = document.getElementById('informerConfirmPassword');
        if (inputPassword.value.length > 0) {
            if (inputPassword.value == inputConfirmPassword.value) {
                $("#divConfirmPassword").removeClass('error');
                $("#divConfirmPassword").addClass('success');
                informerConfirmPassowrd.innerHTML = "Passwords Match! Thank you";
            } else {
                $("#divConfirmPassword").removeClass('success');
                $("#divConfirmPassword").addClass('error');
                informerConfirmPassowrd.innerHTML = "Passwords Do Not Match!";
            }
        }
        else {
            $("#divConfirmPassword").addClass('error');
            informerConfirmPassowrd.innerHTML = "Passwords Do Not Match!";
        }
    });

    /* Drag&Drop */
    $(document).on('drop', 'div.listview', function () {
        ap.Drop(event);
    });

    $(document).on('drop', 'div.listview', function () {
        ap.Drop(event);
    });

    $(document).on('dragover', 'div.listview', function () {
        ap.AllowDrop(event);
    });

    $(document).on('dragover', 'div.listview', function () {
        ap.AllowDrop(event);
    });

    $(document).on('dragstart', 'div.list', function () {
        ap.Drag(event);
    });
    /* Drag&Drop */

    // Create button click
    $(document).on('click', '.button.primary', function () {
        if (this.getAttribute('id') == null) {
            ap.selectedId = null;
            var backupUrl = ap.partialUrl;
            ap.partialUrl = $('#editUrl').val();
            ap.HtmlToDiv();
            ap.partialUrl = backupUrl;
        }
    });
    // Change button click
    $(document).on('click', '.button.success', function () {
        if (this.getAttribute('id') == null) {
            if (ap.selectedId == null) {
                ap.PushMessage('alert', 'No item selected|Please select item to change', false);

            }
            else {
                var backupUrl = ap.partialUrl;
                ap.partialUrl = $('#editUrl').val().replace('UID', ap.selectedId);;
                ap.HtmlToDiv();
                ap.selectedId = null;
                ap.partialUrl = backupUrl;
            }
        }
    });
    // Delete button click
    $(document).on('click', '.button.alert', function () {
        if (ap.selectedId == null) {
            ap.PushMessage('alert', 'No item selected|Please select item to change', false);
        }
        else {
            var dialog = $('#confirmDialog').data('dialog');
            dialog.open();
        }
    });
    // Entries button click
    $(document).on('click', '.button.warning', function () {
        if (ap.selectedId == null) {
            ap.partialUrl = $('#entryUrl').val().replace('UID', -1);
        }
        else
        {
            ap.partialUrl = $('#entryUrl').val().replace('UID', ap.selectedId);
        }
        ap.HtmlToDiv();
        ap.selectedId = null;
        });
    // Close dialog button
    $(document).on('click', '.dialog-close-button', function () {
        $('.list.active').removeClass('active');
        ap.selectedId = null;

    });
    // Cancel button click
    $(document).on('click', '.button.link', function () {
        var id = (this).getAttribute('id');
        if (id.indexOf('cancel') > -1) {
            ap.HtmlToDiv();
            return;
            //alert(id);
        }
        if (id.indexOf('passwordReset') > -1) {
            var dialog = $('#resetDialog').data('dialog');
            dialog.open();
            return;
        }

        if (id.indexOf('closeDialog') > -1) {
            var dialog = $('#resetDialog').data('dialog');
            dialog.close();
            return;
        }

    });
    // Confirm delete button
    $(document).on('click', '.button.alert.block-shadow-error', function () {
        var post = {
            id: ap.selectedId
        };
        var url = $("#deleteUrl").val();
        //alert(url);
        //alert(post.id);
        //return;
        //alert(url + "\r\n" + JSON.stringify(post));
        $.ajax({
            url: url,
            type: 'POST',
            data: post,
            success: function (data) {
                if (data.Result) {
                    ap.PushMessage('success', 'Delete record|Record successfully deleted', false);
                    ap.HtmlToDiv();
                }
                else {
                    var errors = '';
                    data.Errors.forEach(function (error) {
                        errors = errors.concat(error.Description);
                    });
                    ap.PushMessage('alert', 'Delete record|'.concat(errors), false);

                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("error: " + textStatus + "\r\n" + errorThrown + "\r\n" + XMLHttpRequest);

            },
            complete: function () {
                ap.selectedId = null;
                ap.serverId = -1;
            }

        });
    });
    // Confirm reset button
    $(document).on('click', '.button.block-shadow-impact', function () {
        var post = {
            id: $('#userId').val()
        };
        //alert(JSON.stringify(post));
        var url = $("#resetUrl").val();
        $.ajax({
            url: url,
            type: 'POST',
            dataType: "json",
            data: post,
            success: function (data) {
                if (data.Result) {
                    ap.PushMessage('success', 'User edit|<a href="' + data.Errors[0].Description + '">Change Password Url</a>', true);
                    var dialog = $('#resetDialog').data('dialog');
                    dialog.close();
                }
                else {
                    var errors = '';
                    data.Errors.forEach(function (error) {
                        errors = errors.concat(error.Description);
                    });
                    ap.PushMessage('alert', 'User edit|'.concat(errors), false);

                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("error: " + textStatus + "\r\n" + errorThrown + "\r\n" + XMLHttpRequest);

            },
            complete: function () {
                ap.selectedId = null;
            }

        });
    });

});




