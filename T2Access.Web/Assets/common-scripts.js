/*--- AJAX----*/

function AjaxPost(form) {

    //$.validator.unobtrusive.parse(form);
    //if ($(form).valid()) { }

    var ajaxConfig = {

        type: 'POST',
        url: form.action,
        data: new FormData(form),
        success: function (result) {


        }
    }

    if ($(form).attr('enctype') == "multipart/form-data") {

        ajaxConfig["contentType"] = false;
        ajaxConfig["processData"] = false;
    }
    $.ajax(ajaxConfig);

    return false;
}



$("#DTable").on('click', '.btnDelete', function () {

    var btn = this;
    var table = $('#DTable').DataTable();
    
    if (confirm(confirmMessage))
        $.ajax({
            type: 'GET',
            url: deleteUrl + "/" + $(btn).attr("data-id"),
            success: function (result) {
                if (result.success) {
                    table.row($(btn).parents('tr')).remove().draw(false);
                    toastr.success(result.message, "Success")
                } else {
                    toastr.error(result.message, "Error")
                }
            }

        });

});






/*--- Toastr ----*/


var setupToastr = function () {
    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-bottom-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "slideDown",
        "hideMethod": "fadeOut"
    }

}

/*--- BindForm in Modal----*/
var createModal = function () {

    $("#addEditModalContent").load(createUrl, function () {
        $("#addEditModal").modal("show");
        bindForm(this);
    });
}


var ResetPasswordModal = function (data) {
    $("#addEditModalContent").load(resetpasswordUrl, function () {

        $("#Id").val(data.Id);
        $("#UserName").val(data.UserName);
        $("#Password").val(data.Password);
        $("#ConfirmPassword").val(data.ConfirmPassword);

        $("#addEditModal").modal("show");

        bindForm(this);

    });
}




function bindForm(dialog) {
    var table = $('#DTable').DataTable();
    
    
    $('form',dialog).submit(function () {
        var action = this.action;
        var data = $(this).serialize();
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (result.confirm) {
                    $('#addEditModal').modal('hide');
                    ConfirmAdmin(action,data);
                }
                else if (result.success) {
                    $('#addEditModal').modal('hide');

                   // table.clear().destroy();
                    $("#DTable").DataTable().ajax.reload(null,false);

                    //$('#tbodyPartial').load(TableUrl, function () {
                    //   // $("#DTable").DataTable(dataTableConfig).draw(false);
                    //});

                    toastr.success(result.message)
                } else {
                    $('#addEditModalContent').html(result);
                    bindForm(dialog);
                }

            }
        });

        return false;
    });


}

function ConfirmAdmin(returnUrl, dataForm) {

    $("#confirmModalContent").load(ReLoginUrl, function () {
        $('#confirmModal').modal('show');
        bindConfirm(this, returnUrl, dataForm);


    });


}

function bindConfirm(dialog, returnUrl, dataForm) {


    $('form', dialog).submit(function () {

        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (result.success) {
                    $.post(returnUrl, dataForm).done(function (data) {

                        $('#confirmModal').modal('hide');
                        //location.reload();
                    });

                } else {
                    $('#confirmModalContent').html(result);
                    bindConfirm(dialog, dataForm);
                }

            }
        });

        return false;
    });


}











/*---LEFT BAR ACCORDION----*/
$(function() {
  $('#nav-accordion').dcAccordion({
    eventType: 'click',
    autoClose: true,
    saveState: true,
    disableLink: true,
    speed: 'slow',
    showCount: false,
    autoExpand: true,
    //        cookie: 'dcjq-accordion-1',
    classExpand: 'dcjq-current-parent'
  });
});

var Script = function() {


  //    sidebar dropdown menu auto scrolling

  jQuery('#sidebar .sub-menu > a').click(function() {
    var o = ($(this).offset());
    diff = 250 - o.top;
    if (diff > 0)
      $("#sidebar").scrollTo("-=" + Math.abs(diff), 500);
    else
      $("#sidebar").scrollTo("+=" + Math.abs(diff), 500);
  });



  //    sidebar toggle

  $(function() {
    function responsiveView() {
      var wSize = $(window).width();
      if (wSize <= 768) {
        $('#container').addClass('sidebar-close');
        $('#sidebar > ul').hide();
      }

      if (wSize > 768) {
        $('#container').removeClass('sidebar-close');
        $('#sidebar > ul').show();
      }
    }
    $(window).on('load', responsiveView);
    $(window).on('resize', responsiveView);
  });

  $('.fa-bars').click(function() {
    if ($('#sidebar > ul').is(":visible") === true) {
      $('#main-content').css({
        'margin-left': '0px'
      });
      $('#sidebar').css({
        'margin-left': '-210px'
      });
      $('#sidebar > ul').hide();
      $("#container").addClass("sidebar-closed");
    } else {
      $('#main-content').css({
        'margin-left': '210px'
      });
      $('#sidebar > ul').show();
      $('#sidebar').css({
        'margin-left': '0'
      });
      $("#container").removeClass("sidebar-closed");
    }
  });

  // custom scrollbar
  $("#sidebar").niceScroll({
    styler: "fb",
    cursorcolor: "#4ECDC4",
    cursorwidth: '3',
    cursorborderradius: '10px',
    background: '#404040',
    spacebarenabled: false,
    cursorborder: ''
  });

  //  $("html").niceScroll({styler:"fb",cursorcolor:"#4ECDC4", cursorwidth: '6', cursorborderradius: '10px', background: '#404040', spacebarenabled:false,  cursorborder: '', zindex: '1000'});

  // widget tools

  jQuery('.panel .tools .fa-chevron-down').click(function() {
    var el = jQuery(this).parents(".panel").children(".panel-body");
    if (jQuery(this).hasClass("fa-chevron-down")) {
      jQuery(this).removeClass("fa-chevron-down").addClass("fa-chevron-up");
      el.slideUp(200);
    } else {
      jQuery(this).removeClass("fa-chevron-up").addClass("fa-chevron-down");
      el.slideDown(200);
    }
  });

  jQuery('.panel .tools .fa-times').click(function() {
    jQuery(this).parents(".panel").parent().remove();
  });


  //    tool tips

  $('.tooltips').tooltip();

  //    popovers

  $('.popovers').popover();



  // custom bar chart

  if ($(".custom-bar-chart")) {
    $(".bar").each(function() {
      var i = $(this).find(".value").html();
      $(this).find(".value").html("");
      $(this).find(".value").animate({
        height: i
      }, 2000)
    })
  }

}();

jQuery(document).ready(function( $ ) {

  // Go to top
  $('.go-top').on('click', function(e) {
    e.preventDefault();
    $('html, body').animate({scrollTop : 0},500);
  });
});
