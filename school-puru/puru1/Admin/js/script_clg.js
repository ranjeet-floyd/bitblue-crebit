$(document).on('click','.header-name-sub',function(){
	//alert('here');
	event.preventDefault();
	var header_name=$('.header-name').serialize();
	var flag8=0;
	$('.header-name').find('input').each(function(){
		if($(this).val()==null||$(this).val()==""){
			flag8++;
		
		}
	});
	if(flag8==0){
		$.ajax({
			type:'POST',
			datatype:'json',
			url:'db/header-name.php',
			data:{
				header_name:header_name
			},
			success:function(e){
				var str=JSON.parse(e);
				alert(str['status']);
				window.location.reload();
			}
		})
	}
	else{
		alert('Please Enter Name');
	}
});

/* Admin>>create class>> form*/ 
$(document).on('click','.adm_clg_cls_sub',function(event){
	event.preventDefault();
	var adm_clg_cls=$('.adm_clg_cls').serialize();
	var flag2=0;
	$('.adm_clg_cls').find('input').each(function(){
		if($(this).val()==null||$(this).val()==""){
			flag2++;
		
		}
	});
	$('.amd_cls').find('select').each(function(){
		if($(this).val()=="default"){
			flag2++;
		}
	});
	if(flag2==0){
		$.ajax({
			type:'POST',
			datatype:'json',
			url:'db/class.php',
			data:{
				adm_clg_cls:adm_clg_cls
			},
			success:function(e){
				if($(e).length){
					$("div.class-view-result").append(e);
				}
				else{
					alert('details already exists');
				}
		
			}
		});
	}
	else{
		alert('please fill all the details');
	}
});
/*
$(document).on('click','div.class-view-cross',function(e){ 		
	var del_id = $(this).attr('id');
	var mdm=$('li.medium-'+del_id).text();
	var std=$('li.std-'+del_id).text();
	var count=$('li.count-'+del_id).text();
	if(confirm("There are"+count+" students in this class, are you sure you want to delete this class ?")==true){
		$.ajax({
			type:"POST",
			url:"db/class-row-delete.php",
			atync:'true',
			data:{
				mdm:mdm,
				std:std
			},
			success:function(data){
				  $('ul.'+del_id).slideUp('slow');
			}
		});
	}
});	
*/
/* Admin>>Set fee>>form */
$(document).on('click','.adm_set_sub_button',function(event){
	var table_view_id=$('.table-view-id').text();
	event.preventDefault();
	var adm_set=$('.adm_set').serialize();
	var flag3=0;
	$('.adm_set').find('input').each(function(){
		if($(this).val()==null||$(this).val()==""){
			flag3++;
		
		}
	});
	$('.adm_set').find('select').each(function(){
		if($(this).val()=="default"){
			flag3++;
		}
	});
	console.log(table_view_id); 
	if(flag3==0){
		$.ajax({
			type:'POST',
			datatype:'json',
			url:'db/set_fee.php',
			async:'true',
			data:{
				adm_set:adm_set,
				table_view_id:table_view_id
			},
			success:function(d){
			
				if($.trim(d)=='0'){
					alert("no class of this name exists, please add class");
					//window.location.reload();
				}
				else if($.trim(d)=='1'){
					alert("Fee details already exits.");
				}
				else{
				//	console.log(d);
					table_view_id++;
					$("div.table-view-result").append(d);
					$("span.table-view-id").html(++table_view_id);
				}
			}
		});
	}
	else{
		alert("please fill all the details");
	}
});
/*
$(document).on('click','div.table-view-cross',function(e){ 
		var del_id = $(this).attr('id');
		var mdm=$('li.medium-'+del_id).text();
		var std=$('li.std-'+del_id).text();
		$.ajax({
			type:"POST",
			url:"db/row-delete.php",
			atync:'true',
			data:{
				mdm:mdm,
				std:std,
			},
			success:function(data){
				  $('ul.'+del_id).slideUp('slow');
			}
		});
});*/	
/*Admin>>UPdate search student */
$(document).on('click','.adm_upd_show_sub',function(event){
	event.preventDefault();
	var adm_upd_show=$('.adm_upd_show').serialize();
	var flag4=0;
	$('.adm_upd_show').find('input').each(function(){
		if($(this).val()==null||$(this).val()==""){
			flag4++;
		}
	});
	if(flag4==0){
		$.ajax({
			type:'POST',
			datatype:'json',
			url:'db/upd_check.php',
			data:{
				adm_upd_show:adm_upd_show,
				count:'1'
			},
			success:function(e){
				var block=JSON.parse(e);
				if(block['status']=='Suceed'){
						$('div.dyn_upd_details').html(block['data']);	
						console.log(block);
						$('select[name="adm_upd_std"]').val(block["id"]["Std"]);
						$('select[name="adm_upd_mdm"]').val(block["id"]['Medium']);	
						$('select[name="adm_upd_cor"]').val(block["id"]['course']);	
				}
				else{
				$('div.dyn_upd_details').html(e);	
				}
			} 
		});	
	}
	else{
		alert('please fill GR/SR number');
	}
});

/* update student >> search by name*/
$(document).on('click','.adm_upd_show_name_sub',function(event){
	event.preventDefault();
	var adm_upd_show_name=$('.adm_upd_show_name').serialize();
	var flag6=0;
	$('.adm_upd_show_name').find('input').each(function(){
		if($(this).val()==null||$(this).val()==""){
			flag6++;
		}
	});
	if(flag6==0){
		$.ajax({
			type:'POST',
			datatype:'json',
			url:'db/upd_check_name.php',
			data:{
				adm_upd_show_name:adm_upd_show_name
			},
			success:function(e){
				$('div.dyn_upd_details_name').html(e);	
			}
		});	
	}
	else{
		alert('please Enter Name');
	}
});

$(document).on('click','.name-search-sub',function(event){
	event.preventDefault();
	var srch_id=$(this).attr('id');
	var adm_upd_show=$('.'+srch_id).text();
	//console.log(adm_upd_show);
	$.ajax({
		type:'POST',
		datatype:'json',
		url:'db/upd_check.php',
		data:{
			adm_upd_show:adm_upd_show,
			count:'2'
		},
		success:function(e){
			var block=JSON.parse(e);
			if(block['status']=='Suceed'){
					$('div.dyn_upd_details').html(block['data']);
				//	console.log(block['id']);
					$('select[name="adm_upd_std"]').val(block["id"]["Std"]);
					$('select[name="adm_upd_mdm"]').val(block["id"]['Medium']);
					$('select[name="adm_upd_cor"]').val(block["id"]['course']);	
					$('select[name="adm_upd_sex"]').val(block["det"]['sex']);
					$('select[name="adm_upd_tsta"]').val(block["det"]['status']);
			}
			else{
			$('div.dyn_upd_details').html(e);
				console.log(e);
				//alert("Invalid GR Number")
			//	window.location.reload();
			}
		}
	});	
});

 /*Admin>>Update student>>update student*/
  $(document).on('click','.adm_upd_sub',function(event){
	event.preventDefault();
	var adm_grn=$('.adm_upd_grn_span').text();
	console.log(adm_grn);
	var flag9=0;
	var adm_upd=$('.adm_upd_std_form').serialize();
	$('form.adm_upd_std_form').find('.mando').each(function(){
			if($(this).val()==null||$(this).val()==""){
				console.log($(this).val());
				flag9++;
			
			}
	});
	var phoneNumber = /\(?([0-9]{3})\)?([ .-]?)([0-9]{3})\2([0-9]{4})/;
	var inputpn=$('.inputpn').val();
	//console.log(phoneNumber.test(inputpn));
	if((phoneNumber.test(inputpn)==false)){  
		alert('Enter valid Phone Number');  
		return false;
	}
	if(flag9==0){
		$.ajax({
			type:'POST',
			datatype:'json',
			url:'db/upd_std.php',
			data:{
				adm_upd:adm_upd,
				adm_grn:adm_grn
			},
			success:function(e){
				var check=JSON.parse(e);
				console.log(check);
				if(check['status']=='suceed'){
					alert('Result Updated');
					window.location.reload();
				}
				else{
					alert('No class of this description available');
				}
			}
		});
	}
	else{
		alert("Please Fill mandtory details");
	}
});

/* Admin>>Create USER*/
$(document).on('click','.adm_cusr_sub',function(event){
	event.preventDefault();
	var flag1=0;
	var adm_cusr=$('.adm_cusr').serialize();
	$('.adm_cusr').find('input').each(function(){
			if($(this).val()==null||$(this).val()==""){
				flag1++;
			
			}
		});
		$('.adm_cusr').find('select').each(function(){
			if($(this).val()=="default"){
				flag1++;
			}
		});
		var phoneNumber = /\(?([0-9]{3})\)?([ .-]?)([0-9]{3})\2([0-9]{4})/;
		var inputpn=$('.inputpn1').val();
		console.log(phoneNumber.test(inputpn));
		if((phoneNumber.test(inputpn)==false)){  
			alert('Enter valid Phone Number');  
			return false;
        }
	//console.log(adm_cusr);
	if(flag1==0){
		$.ajax({
			type:'POST',
			datatype:'json',
			url:'db/crt_user.php',
			data:{
				adm_cusr:adm_cusr
			},
			success:function(e){
				var s=JSON.parse(e);
				if(s['status']=='suceed'){
					alert('user added');
					window.location.reload();
				}
				else{
					alert('choose different username');
				}
			}
		});
	}
	else{
		alert('please fill mandatory details');
	}
});

/* Search students*/

$(document).on('click','.adm_sch_src_sub',function(event){
	event.preventDefault();		
	var adm_src=$('.adm_sch_src').serialize();
	flag=0;
	$('.adm_sch_src').find('input').each(function(){
		if($(this).val()==null||$(this).val()==""){
			flag++;
		
		}
	})
	if(flag==0){
		$.ajax({
			type:'POST',
			datatype:'json',
			url:'db/src_std.php',
			data:{
				adm_src:adm_src,
				count:'1'
			},
			success:function(e){
				$('div.search-div').html(e);
				//window.location.reload();
			}
		});
	}
	else{
		alert('please fill GR/SR number');
	}
});

/*search student by name */
$(document).on('click','.adm_sch_src_name_sub',function(event){
	event.preventDefault();
	var adm_sch_src_name=$('.adm_sch_src_show_name').serialize();
	var flag7=0;
	$('.adm_sch_src_show_name').find('input').each(function(){
		if($(this).val()==null||$(this).val()==""){
			flag7++;
		}
	});
	if(flag7==0){
		$.ajax({
			type:'POST',
			datatype:'json',
			url:'db/src_std_name.php',
			data:{
				adm_sch_src_name:adm_sch_src_name
			},
			success:function(e){
				$('div.dyn_src_details_name').html(e);	
			}
		});	
	}
	else{
		alert('please Enter Name');
	}
});

$(document).on('click','.name-search-1-sub',function(event){
	event.preventDefault();		
	var adm_src=$('.'+$(this).attr('id')).text();
	$.ajax({
		type:'POST',
		datatype:'json',
		url:'db/src_std.php',
		data:{
			count:'2',
			adm_src:adm_src
		},
		success:function(e){
			$('div.search-div').html(e);
			//window.location.reload();
		}
	});

});

/* revenue.php*/
$(document).on('click','.rev-revenue',function(){
	$.ajax({
		url:'db/rev-revenue.php',
		method:'POST',
		datatype:'json',
		success:function(e){
			$('.rev-div-main').html(e);
		}
	});
});
$(document).on('click','.rev-recieve',function(){
	$.ajax({
		url:'db/rev-recieve.php',
		method:'POST',
		datatype:'json',
		success:function(e){
			$('.rev-div-main').html(e);
		}
	});
});

$(document).on('click','li.logout-span',function(){
	//alert('here');
	$.ajax({
		url:'logout.php',
		method:'POST',
		datatype:'json',
		success:function(e){
			alert('Logged off successfully');
			window.location='index.php';
		}
	});
});
/* Transaction Page*/
$(document).on('click','.srch-fee-sub',function(){
	var strdate=$('input[name="srch-str-date"]').val();
	var enddate=$('input[name="srch-end-date"]').val();
	console.log(strdate);
	$.ajax({
		url:'db/tran.php',
		method:'POST',
		datatype:'json',
		data:{
			enddate:enddate,
			strdate:strdate
		},
		success:function(e){
			$('div.show-by-date').html(e);
		}
	});
});
$(document).on('click','.srch-mode-sub',function(){
	//alert('here');
	event.preventDefault();
	var mode=$('select.srch-mode-name').val();
	console.log(mode);
	$.ajax({
		url:'db/tran-name.php',
		method:'POST',
		datatype:'json',
		data:{
			mode:mode		},
		success:function(e){
			$('div.show-by-date').html(e);
		}
	});
});

/* Pending Transactions*/

$(document).on('click','.rev-pend',function(){
	//alert('here');
	$.ajax({
		url:'db/pend.php',
		method:'POST',
		datatype:'json',
		success:function(e){
			$('div.rev-div-main').html(e);
		}
	});
});

