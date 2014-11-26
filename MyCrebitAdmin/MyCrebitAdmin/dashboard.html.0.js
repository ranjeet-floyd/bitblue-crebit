
  function hideBank()
  {
    $("#link_page").text("Electricity");
    $("#a_electricity").addClass('active');
    $("#a_bank").removeClass('active');
    $("#bank-details").addClass("hideMe").removeClass('showMe');
    $("#electricity-details").addClass("showMe").removeClass("hideMe");
  }
  function hideELectricity()
  {
    $("#a_electricity").removeClass('active');
    $("#a_bank").addClass('active');
    $("#link_page").text("Bank Details");
    $("#bank-details").addClass("showMe").removeClass("hideMe");
    $("#electricity-details").addClass("hideMe").removeClass("showMe");
  }
