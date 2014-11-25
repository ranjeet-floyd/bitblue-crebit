/* Add student*/	
	$(document).on('click','.clg_ads_sub',function(event){
		var add_std=$('.clg_ads').serialize();
		var flag2=0;
		event.preventDefault();
		$('.clg_ads').find('.mando').each(function(){
			if($(this).val()==null||$(this).val()==""){
				flag2++;
			
			}
		});
		$('.clg_ads').find('.mando').each(function(){
			if($(this).val()=="default"){
				flag2++;
			}
		});
		var phoneNumber = /\(?([0-9]{3})\)?([ .-]?)([0-9]{3})\2([0-9]{4})/;
		var inputpn=$('.inputpn').val();
		//console.log(phoneNumber.test(inputpn));
		if((phoneNumber.test(inputpn)==false)){  
			alert('Enter valid Phone Number');  
			return false;
        }
		if(flag2==0){
		$.ajax({
			type:'POST',
			datatype:'json',
			url:'db/add_std.php',
			data:{
				add_std:add_std
				}
		}).done(function(data){
			var arr=JSON.parse(data)
		    console.log(arr);
			if((arr['status'])!='GRN'){
				alert(arr['status']);
				window.location.href = window.location.href;
			}
			else{
				alert('GR/SR number already exists please choose a new one')
			}
		})
		}
		else{
			alert('fll all the details');
		}
	});
/* school>> show update for class*/
	 $(document).on('click','.sch_updc_show_sub',function(event){
		var sch_updc_show=$('.sch_updc_show').serialize();
		event.preventDefault();
		var flag1=0;
		event.preventDefault();
		$('.sch_updc_show').find('input').each(function(){
			if($(this).val()==null||$(this).val()==""){
				flag1++;
			
			}
		});
		$('.sch_updc_show').find('select').each(function(){
			if($(this).val()=="default"){
				flag1++;
			}
		});
		if(flag1==0){
			$.ajax({
				type:'POST',
				datatype:'html',
				url:'db/updc_show.php',
				data:{
					sch_updc:sch_updc_show
					}
			}).done(function(data){
				$('div.updc_show_div').html(data);
			})
		}
		else{
			alert('please fill required fields');
		}
	});
	$(document).on('click','.check-select-all',function(){
		 var checkboxes = new Array();
		 var val=$(this).val();
		  $('input[name='+val+']').attr('checked', this.checked);
	});
	
	/* updating the class*/
	$(document).on('click','.updc_class',function(event){
		var id=$(this).attr('id');
		var check = new Array();
		$("input[name="+id+"]:checked").each(function() {
		   check.push($(this).val());
		});
		var nclass=$("div."+id+" select[name='update-to']").val();
		var nmdm=$("div."+id+" select[name='update-mdm']").val();
		var nsec=$("div."+id+" select[name='update-sec']").val();
		var ncor=$("div."+id+" select[name='update-cor']").val();
		//console.log(ncor);
		event.preventDefault(event);
		if(check.length === 0){
				alert('please select Students');
		}
		else{
			$.ajax({
				type:'POST',
				datatype:'json',
				url:'db/updc.php',
				data:{
					updc:check,
					nclass:nclass,
					ncor:ncor,
					nmdm:nmdm,
					nsec:nsec
					}
			}).done(function(data){
				var sta=JSON.parse(data);
				alert(sta['status']);
				if(sta['status']=='suceed'){
				window.location.href = window.location.href;
				}
				})
		}
	});
		/* add fee */
	$(document).on('click','.sch_fee_grn_sub',function(event){
		event.preventDefault(event);		
		var fee_grn=$('.sch_fee_grn').serialize();
		$.ajax({
			type:'POST',
			datatype:'json',
			url:'db/sch_fee_show.php',
			data:{
				fee_show:fee_grn
			},
			success:function(e1){
				$('div.sch_fee_div').html(e1);
				//window.location.reload();
			}
		});
	});

	$(document).on('click','.sch_fee_verify',function(){
		event.preventDefault();
		var flag=0;
		var fee_chng_grn=$('span.fee_chng_grn').text();
		var fee_chng=$('.fee_chng_form').serialize();
		//console.log(fee_chng);
		$('.fee_chng_form').find('input').each(function(){
			if($(this).val()==null||$(this).val()==""){
				flag++;
				console.log('1');
			}
		});
		if($('input.sel_mon').length){
			if(!$('input.sel_mon').is(':checked')){
				flag++;
				console.log('2');
			}
		}
		$('.fee_chng_form').find('select').each(function(){
			if($(this).val()=="default"){
				flag++;
				console.log('4');
			}
		});
		var fee_chng_amount=$('.fee_chng_form_hide_1').text();
		var fee_chng_lfee=$('.fee_chng_form_hide_2').text();
		//console.log(flag);
		if(flag==0){
			$.ajax({
				type:'POST',
				datatype:'json',
				url:'db/fee_chng_pop.php',
				data:{
					fee_chng:fee_chng,
					fee_chng_grn:fee_chng_grn,
					fee_chng_lfee:fee_chng_lfee,
					fee_chng_amount:fee_chng_amount
				},
				success:function(e){
					var c=JSON.parse(e);
					if(c['status']=='suceed'){
						alert("Fee added");
						console.log(c);
						$.ajax({
							type:'POST',
							datatype:'json',
							url:'db/print.php',
							data:{
								c:c
							},
							success:function(f){
								$('.print_rec').html(f);
								
							}
						});	
					}
					else{
						alert('Duplicate entry, please delete duplicate value from "Search Student" to make changes');
						//$('.fee_chng_form').trigger('reset');
					}
				}
			});
		}
		else{
			alert('please fill mandatory fields');
		}
	});
	
 function cheque(){
	var c_show=$('select[name="fee_chng_form_paym"]').val();
	
	if(c_show=='cheque'){	
	$('input[name="fee_chng_form_chq"]').css({'visibility':'visible'});
	}
	else{
		$('input[name="fee_chng_form_chq"]').css('visibility','hidden');
	}
}
function fee_type(){
	var fee_chng_grn=$('span.fee_chng_grn').text();
	var fee_chng_mdm=$('span.fee_chng_mdm').text();
	var fee_chng_cor=$('span.fee_chng_cor').text();
	var fee_chng_std=$('span.fee_chng_std').text();
	var fee_chng_type=$('select[name="fee_chng_form_fee_type"]').val();
	//console.log(fee_chng_cor);
	$.ajax({
		type:'POST',
		datatype:'json',
		url:'db/fee_chng_type.php',
		data:{
			fee_chng_type:fee_chng_type,
			fee_chng_mdm:fee_chng_mdm,
			fee_chng_cor:fee_chng_cor,
			fee_chng_std:fee_chng_std,
			fee_chng_grn:fee_chng_grn
		},
		success:function(e){
			var chck=JSON.parse(e);
			//console.log(chck);
			if(chck['status']=="suceed"){
				$('.fee_chng_form_hide').html(chck['data']);
				$('.fee_chng_form_hide_1').html(chck['fee']);
				$('.fee_chng_form_hide_4').html(chck['fee']);
				$('.fee_chng_form_hide_2').html(chck['lfee']);
			}
			else{
				alert('No fee found to matching criteria, please add fee via admin panel');
				$('.fee_chng_form').trigger('reset');
			}
		}
	});
}
/* add fee -> amount monthly fee*/
 $(document).on('click','.sel_mon',function(){
	//alert('here');
	var count=0;
	$('span.fee_chng_form_hide').find('.sel_mon').each(function(){
		if($(this).is(':checked')){
			count++;
		}
	});
	var val=$('.fee_chng_form_hide_4').text();
	val=val*count;
	if(count>0){
		$('.fee_chng_form_hide_1').html(val);
	}
 });
 
 
 /*add fee-> late fee column */
 $(document).on('change','select[name="fee_chng_form_ot"]',function(){
	if($(this).val()=='no'){
		$('.fee_chng_form_hide_2').css('display','inherit');
	}
	else{
		$('.fee_chng_form_hide_2').css('display','none');
	}
 });
 /* add feee-> late fee column->monthly fee*/
 $(document).on('click','.sel_late',function(){
	//alert('hurrah');
	var count=0;
	$('span.fee_chng_form_hide').find('.sel_late').each(function(){
		if($(this).is(':checked')){
			count++;
		}
	});
	var val=$('.fee_chng_form_hide_2').text();
	//console.log(val);
	val=val*count;
	//console.log(val);
	if(count>0){
		$('.fee_chng_form_hide_3').html(val);
		$('.fee_chng_form_hide_3').css('display','inherit');
	}
	else{
		$('.fee_chng_form_hide_3').css('display','none');
	}
 });
 
 $(document).on('click','.print_add_fee',function(){
	window.print();
 });
/* search students*/
$(document).on('click','.sch_src_sub',function(event){
	event.preventDefault();		
	var sch_src=$('.sch_src').serialize();
	flag=0;
	$('.sch_src').find('input').each(function(){
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
				sch_src:sch_src,
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
$(document).on('click','.sch_src_name_sub',function(event){
	event.preventDefault();
	var sch_src_name=$('.sch_src_show_name').serialize();
	var flag7=0;
	$('.sch_src_show_name').find('input').each(function(){
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
				sch_src_name:sch_src_name
			},
			success:function(e){
				$('div.sch_src_details_name').html(e);	
			}
		});	
	}
	else{
		alert('please Enter Name');
	}
});

$(document).on('click','.name-search-2-sub',function(event){
	event.preventDefault();
	console.log($(this).attr('id'));
	var sch_src=$('.'+$(this).attr('id')).text();
	console.log(sch_src);
	$.ajax({
		type:'POST',
		datatype:'json',
		url:'db/src_std.php',
		data:{
			count:'2',
			sch_src:sch_src
		},
		success:function(e){
			$('div.search-div').html(e);
			//window.location.reload();
		}
	});

});


$(document).on('click','.delete-fee',function(event){
	event.preventDefault();
	var id=$(this).attr('id');
//	console.log(id);
	var fee_rec=$('.rec-'+id).text();
	var fee_typ=$('.feet-'+id).text();
	var fee_grn=$('.src_grn').text();
	var fee_mon=$('.mon-'+id).text();
	//console.log(fee_typ);
	$.ajax({
		type:'POST',
		datatype:'json',
		url:'db/delete-fee.php',
		data:{
			fee_typ:fee_typ,
			fee_grn:fee_grn,
			fee_rec:fee_rec,
			fee_mon:fee_mon
		},
		success:function(e){
		//console.log(id);
			$('ul.'+id).slideUp('slow');
		//	alert('Fee entry Deleted');
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
			alert('logged off successfully');
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
 /* division dynamic*/
function div_count(){
	var mdm=$("select[name='clg_ads_mdm']").val();
	var std=$("select[name='clg_ads_std']").val();
	var cor=$("select[name='clg_ads_cor']").val();
//	console.log(mdm,std)
	$.ajax({
		url:'db/div_count.php',
		method:'POST',
		datatype:'json',
		data:{
			mdm:mdm,
			std:std,
			cor:cor
		},
		success:function(e){
			var div=JSON.parse(e);
			var block='Division: <select name="sch_ads_div" class="mando">';
			$.each(div['data'],function(index,value){
				block=block+'<option value='+value+'>'+value+'</option>'
			});
			block=block+'</select>';
			//console.log(block);
			$("div.div-update-show").html(block);
		}
	});
}

function div_count1(){
	var mdm=$("select[name='sch_updc_show_mdm']").val();
	var std=$("select[name='sch_updc_show_std']").val();
	var cor=$("select[name='sch_updc_show_cor']").val();
	//console.log(mdm,std)
	$.ajax({
		url:'db/div_count.php',
		method:'POST',
		datatype:'json',
		data:{
			mdm:mdm,
			std:std,
			cor:cor
		},
		success:function(e){
			var div=JSON.parse(e);
			var block='Division: <select name="sch_updc_show_div" class="mando">';
			$.each(div['data'],function(index,value){
				block=block+'<option value='+value+'>'+value+'</option>'
			});
			block=block+'</select>';
			//console.log(block);
			$("div.div-update-show").html(block);
		}
	});
}
$(document).on('change','.update-to',function(){
	//alert('here');
	var mdm=$("select[name='update-mdm']").val();
	var std=$("select[name='update-to']").val();
	var cor=$("select[name='update-cor']").val();
	//console.log(mdm,std)
	//alert('here');
	$.ajax({
		url:'db/div_count.php',
		method:'POST',
		datatype:'json',
		data:{
			mdm:mdm,
			std:std,
			cor:cor
		},
		success:function(e){
			var div=JSON.parse(e);
			var block='Division: <select name="sch_updc_show_div" class="mando">';
			$.each(div['data'],function(index,value){
				block=block+'<option value='+value+'>'+value+'</option>'
			});
			block=block+'</select>';
			//console.log(block);
			$("select[name='update-sec']").html(block);
		}
	});
});