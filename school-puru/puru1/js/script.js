
/* main LOGIN page*/

$(document).on('click','.login-sub',function(event){
	event.preventDefault();
	var login=$('.login-main').serialize();
	var flag5=0;
	$('.login-main').find('input').each(function(){
		if($(this).val()==null||$(this).val()==""){
			flag5++;
		}
	});
	$('.login-main').find('select').each(function(){
			if($(this).val()=="default"){
				flag5++;
			}
		});
		//console.log(login);
	if(flag5==0){
		$.ajax({
			type:'POST',
			datatype:'json',
			url:'check.php',
			data:{
				login:login
			},
			success:function(e){
				//console.log(e);
				var entry=JSON.parse(e);
				//console.log(entry['status']);
				if(entry['status']=='suceed'){
					window.location=''+entry['type']+'/index.php';
					console.log(''+entry['type']+'/index.php');
				}
				else{
					alert('Invalid Details');
				}
			}
		})
	}
	else{
		alert('please enter login details');
	}
});
$(document).on('click','li.logout-span',function(){
	//alert('here');
	$.ajax({
		url:'logout.php',
		method:'POST',
		datatype:'json',
		success:function(e){
			window.location='index.php';
		}
	});
});


