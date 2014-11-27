Kuler.waitHtml = '<span class="wait"><img src="catalog/view/theme/default/image/loading.gif" /></span>';

(function () {
	var $notification = $('#notification'),
		timer;

	if (Kuler.show_custom_notification) {
		Kuler.notification_show_time = parseInt(Kuler.notification_show_time) || 2500;
	}

	function kulerAlert(type, message) {
		if (!$notification.length) {
			$notification = $('#notification');
		}

		clearTimeout(timer);

		$notification
			.html('<div class="'+ type +'">' + message + '<img src="catalog/view/theme/default/image/close.png" class="close" /></div>');

		if (Kuler.show_custom_notification) {
			$notification
				.addClass('active')
				.children()
					.css('display', 'none')
					.fadeIn('slow', function () {
						timer = setTimeout(function () {
							$notification.removeClass('active');
						}, Kuler.notification_show_time);
					});
		} else {
			$('html, body').animate({ scrollTop: 0 }, 'slow');
		}
	}

	window.kulerAlert = kulerAlert;
})();

var _cartTimer;
function addToCart(product_id, quantity) {
	quantity = typeof(quantity) != 'undefined' ? quantity : 1;

	clearTimeout(_cartTimer);

	$.ajax({
		url: 'index.php?route=checkout/cart/add',
		type: 'post',
		data: 'product_id=' + product_id + '&quantity=' + quantity,
		dataType: 'json',
		success: function(json) {
			$('.success, .warning, .attention, .information, .error').remove();

			if (json['redirect']) {
				location = json['redirect'];
			}

			if (json['success']) {
				kulerAlert('success', json['success']);
				Kuler.cart_product_total += quantity;

				$('#cart-total').html(json['total']);
				$('#cart-product-total').html(Kuler.cart_product_total);
			}
		}
	});
}

var _wishListTimer;
function addToWishList(product_id) {
	clearTimeout(_wishListTimer);

	$.ajax({
		url: 'index.php?route=account/wishlist/add',
		type: 'post',
		data: 'product_id=' + product_id,
		dataType: 'json',
		success: function(json) {
			$('.success, .warning, .attention, .information').remove();

			if (json['success']) {
				kulerAlert('success', json['success']);

				$('#wishlist-total').html(json['total']);
			}
		}
	});
}

var _compareTimer;
function addToCompare(product_id) {
	clearTimeout(_compareTimer);

	$.ajax({
		url: 'index.php?route=product/compare/add',
		type: 'post',
		data: 'product_id=' + product_id,
		dataType: 'json',
		success: function(json) {
			$('.success, .warning, .attention, .information').remove();

			if (json['success']) {
				kulerAlert('success', json['success']);

				$('#compare-total').html(json['total']);
			}
		}
	});
}

if (Kuler.show_quick_view) {
    function initQuickView(selector) {
	    $(selector).on('click', function (evt) {
		    evt.preventDefault();

		    var $el = $(this);

		    $.magnificPopup.open({
			    items: {
				    src: this.href || $el.data('href')
			    },
			    type: 'iframe',
			    mainClass: 'mfp-fade'
		    });
        });
    };
}

jQuery(document).ready(function ($) {
	var $window = $(window);

	// Fixed Header
	if (Kuler.fixed_header) {
		var $fixedEl = $('#header');

		$window.bind('scroll', function() {
			if ($window.scrollTop() > 50) {
				$fixedEl.addClass('fix');
			} else {
				$fixedEl.removeClass('fix');
			}
		});
	}

    // Currency
    $('#currency a').on('click', function(e) {
        e.preventDefault();

        $('#currency input[name=\'currency_code\']').attr('value', $(this).attr('href'));

        $('#currency').submit();
    });

    // Language
    $('#language a').on('click', function(e) {
        e.preventDefault();

        $('#language input[name=\'language_code\']').attr('value', $(this).attr('href'));

        $('#language').submit();
    });

	// Quick View
	if (Kuler.show_quick_view) {
        initQuickView('.quick-view a');
	}

	// Newsletter
	if (Kuler.show_newsletter) {
		$('#newsletter-form').on('submit', function () {
			var $mail = $('#newsletter-mail'),
				$button = $('#newsletter-submit'),
				mail = $mail.val();

			if (!mail) {
				return false;
			}

			$mail.prop('disabled', true);
			$button.prop('disabled', true);
			$button.after(Kuler.waitHtml);

			$.post(Kuler.newsletter_subscribe_link, {
				mail: mail
			}, function (data) {
				var type = data.status ? 'success' : 'error';

				kulerAlert(type, data.message);

				$mail.prop('disabled', false);
				$button.prop('disabled', false);

				$button.next().remove();
			}, 'json');

			return false;
		});
	}

	//Smooth scroll to on page element
    $(".review a").click(function(event){
        event.preventDefault();
        //calculate destination place
        var dest=0;
        if($(this.hash).offset().top > $(document).height()-$(window).height()){
            dest=$(document).height()-$(window).height();
        } else {
            dest=$(this.hash).offset().top;
        }
        //go to destination
        $('html,body').animate({scrollTop:dest}, 500,'swing');
     });

	// Login Popup
	if (Kuler.login_popup) {
		$('a').each(function () {
			if (this.href == Kuler.login_url) {
				$(this).on('click', function (evt) {
					evt.preventDefault();

					$.magnificPopup.open({
						items: [
							{
								src: '#login-popup'
							}
						],
						type: 'inline',
						mainClass: 'mfp-fade'
					});
				});
			}
		});
	}

	var $popupLoginForm = $('#popup-login-form');
	$popupLoginForm.on('submit', function (evt) {
		evt.preventDefault();

		$.ajax({
			url: Kuler.popup_login_url,
			type: 'POST',
			dataType: 'json',
			data: $popupLoginForm.serialize(),
			beforeSend: function () {
				$popupLoginForm.find('[type="submit"]').after(Kuler.waitHtml);
				$popupLoginForm.find('.warning').remove();
			},
			complete: function () {
				$popupLoginForm.find('.wait').remove();
			},
			success: function (data) {
				if (data.status) {
					location.reload();
				} else {
					$popupLoginForm.prepend('<div class="warning">'+ data.message +'</div>');
				}
			}
		})
	});

	// Scroll up
	if (Kuler.enable_scroll_up) {
		var $scrollup = $('.scrollup');

		$window.scroll(function() {
			if ($window.scrollTop() > 100) {
				$scrollup.addClass('show');
			} else {
				$scrollup.removeClass('show');
			}
		});

		$scrollup.on('click', function(evt) {
			$("html, body").animate({ scrollTop: 0 }, 600);
			evt.preventDefault();
		});
	}

	if (Kuler.category_menu_type === 'accordion') {
		var $boxCategory = $('.box-category');

		$('.box-category .toggle').on('click', function () {
			var $this = $(this);

			$boxCategory
				.find('li.active')
					.removeClass('active')
					.find('ul')
						.slideUp();
			$this.next().slideDown();
			$this.parent().addClass('active');
		});
	}

    $('.dropdown-toggle').on('click', function () {
        $(this).next().toggleClass('open');
    });

	// Setup mobile main menu
    $('#btn-mobile-toggle').toggle(
        function() {
            $(this).removeClass('expand').next().slideUp();
        },
        function() {
            $(this).addClass('expand').next().slideDown();
        }
    );

    $('.btn-expand-menu').toggle(
        function() {
            $(this).next().slideUp().parent().removeClass('expand');
        },
        function() {
            $(this).next().slideDown().parent().addClass('expand');
        }
    );

    var innerWidth = $(window).innerWidth();

    if (innerWidth < 768) {
        $('#btn-mobile-toggle').trigger('click');
        $('.btn-expand-menu').trigger('click');
    }

    // Setup mobile tabs
    $('#btn-tabs-toggle').toggle(
        function() {
            $(this).parent().removeClass('collapse').addClass('expand').find('.ui-state-default').slideDown();
        },
        function() {
            $(this).parent().removeClass('expand').addClass('collapse').find('.ui-state-default:not(.ui-state-active)').slideUp();
        }
    );

    $('.product-grid .col-lg-3').mouseover(function(){
        $(this).addClass('over').removeClass('out');
    });

    $('.product-grid .col-lg-3').mouseleave(function(){
        $(this).removeClass('over').addClass('out');
    });
});
